using System.Net;
using helpdesk_prove_request.Model;
using helpdesk_prove_request.Model.Callback;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace helpdesk_prove_request.Controllers;
public class CallbackController : Controller
{
    private enum RequestType
    {
        Unknown,
        Presentation,
        Issuance,
        Selfie
    };
    protected IMemoryCache _cache;
    protected readonly ILogger<CallbackController> _log;
    private string _apiKey;
    private IConfiguration _configuration;

    public CallbackController(IConfiguration configuration, IMemoryCache memoryCache, ILogger<CallbackController> log)
    {
        _configuration = configuration;
        _cache = memoryCache;
        _log = log;
        _apiKey = System.Environment.GetEnvironmentVariable("API-KEY") ?? string.Empty;
    }

    private async Task<ActionResult> HandleRequestCallback(RequestType requestType)
    {
        // Initialize variables
        bool validRequest = false;
        string? errorMessage = null;

        // Check if the API key is present in the evironment variables
        if (string.IsNullOrEmpty(_apiKey))
        {
            _log.LogError("API key is not set in the environment variables.");
            return new ContentResult() { StatusCode = (int)HttpStatusCode.Unauthorized, Content = "API key is not set in the environment variables." };
        }

        try
        {
            // Check if the API key is present in the request headers and if it matches the one in the environment variables
            this.Request.Headers.TryGetValue("api-key", out var apiKey);
            if (requestType != RequestType.Selfie && this._apiKey != apiKey)
            {
                _log.LogTrace("api-key wrong or missing");
                return new ContentResult() { StatusCode = (int)HttpStatusCode.Unauthorized, Content = "api-key wrong or missing" };
            }

            // Read the request body and parse it into a CallbackData object
            string body = await new System.IO.StreamReader(this.Request.Body).ReadToEndAsync();
            _log.LogTrace(body);
            CallbackData callback = CallbackData.Parse(body);

            // Define the expected request statuses for each request type
            List<string> presentationStatus = new List<string>() { "request_retrieved", "presentation_verified", "presentation_error" };
            List<string> issuanceStatus = new List<string>() { "request_retrieved", "issuance_successful", "issuance_error" };
            List<string> selfieStatus = new List<string>() { "selfie_taken" };

            // Check if the request type is valid and if the request status is in the expected list
            if ((requestType == RequestType.Presentation && presentationStatus.Contains(callback.RequestStatus))
                || (requestType == RequestType.Issuance && issuanceStatus.Contains(callback.RequestStatus))
                || (requestType == RequestType.Selfie && selfieStatus.Contains(callback.RequestStatus)))
            {
                if (!_cache.TryGetValue(callback.State, out string? requestState))
                {
                    errorMessage = $"Invalid state '{callback.State}'";
                }
                else
                {
                    // Update the state data with the new status and callback information
                    StateData stateData = StateData.Parse(requestState!);
                    stateData.Status = callback.RequestStatus;
                    stateData.Callback = body;

                    switch (stateData.Status)
                    {
                        case "request_created":
                            stateData.Message = "Waiting for the user to open the notification.";
                            break;
                        case "request_retrieved":
                            stateData.Message = "The user has opened the notification. Awaiting completion of their verification.";
                            break;
                        case "issuance_error":
                            stateData.Message = "Issuance failed: " + callback.Error.Message;
                            break;
                        case "issuance_successful":
                            stateData.Message = "Issuance successful";
                            break;
                        case "presentation_error":
                            stateData.Message = "Presentation failed:" + callback.Error.Message;
                            break;
                        case "presentation_verified":
                            stateData.Message = "The user has successfully completed their verification process.";
                            stateData.Type = callback.VerifiedCredentialsData[0].Type;
                            stateData.Claims = callback.VerifiedCredentialsData[0].Claims;
                            stateData.Subject = callback.Subject;
                            break;
                    }

                    // Update the state data in the cache with a new status
                    _cache.Set(callback.State,
                        stateData.ToString(),
                        DateTimeOffset.Now.AddSeconds(_configuration.GetValue<int>("AppSettings:CacheExpiresInSeconds", 300)));

                    // Set the response to be valid
                    validRequest = true;
                }
            }
            else
            {
                // Handle unknown request statuses
                errorMessage = $"Unknown request status '{callback.RequestStatus}'";
            }

            // If the request is not valid, return a bad request response with the error message
            if (!validRequest)
            {
                return BadRequest(new { error = "400", error_description = errorMessage });
            }

            return new OkResult();
        }
        catch (Exception ex)
        {
            // Handle exceptions and return a bad request response with the exception message
            return BadRequest(new { error = "400", error_description = ex.Message });
        }
    }


    [AllowAnonymous]
    [HttpPost("/api/verifier/presentationcallback")]
    public async Task<ActionResult> PresentationCallback()
    {
        return await HandleRequestCallback(RequestType.Presentation);
    }
}
