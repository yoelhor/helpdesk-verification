
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using helpdesk_prove_request.Helpers;
using helpdesk_prove_request.Model;
using helpdesk_prove_request.Model.Ajax;
using helpdesk_prove_request.Model.Presentation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace helpdesk_prove_request.Controllers;

public class VerifierController : Controller
{
    private readonly ILogger<VerifierController> _logger;
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _cache;

    public VerifierController(ILogger<VerifierController> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory, IMemoryCache cache)
    {
        _logger = logger;
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
        _cache = cache;
    }

    [AllowAnonymous]
    [HttpPost("/api/verifier/presentation-request")]
    public async Task<ActionResult> PresentationRequest([FromBody] AjaxPayload ajaxPayload)
    {
        // Validate the input payload
        if (ajaxPayload == null || string.IsNullOrWhiteSpace(ajaxPayload.Upn))
        {
            return BadRequest(new { error = "400", error_description = "Invalid payload UPN is required" });
        }

        try
        {
            // Acquire an access token using the client credentials flow
            var accessToken = await MsalAccessTokenHandler.GetAccessToken(_configuration);
            if (accessToken.Item1 == String.Empty)
            {
                _logger.LogError($"Failed to acquire an access token: {accessToken.error} : {accessToken.error_description}");
                return BadRequest(new { error = accessToken.error, error_description = accessToken.error_description });
            }

            // Create a presentation request and log the payload
            PresentationRequest request = CreatePresentationRequest(ajaxPayload);
            string jsonString = request.ToString();

            // Log the request payload
            _logger.LogTrace($"Request API payload: {jsonString}");

            // Create the HTTP client and set the authorization header
            string url = $"{_configuration["VerifiedID:ApiEndpoint"]}createPresentationRequest";
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.token);

            // Call the Verified ID API to start a presentation request
            HttpResponseMessage res = await client.PostAsync(url, new StringContent(jsonString, Encoding.UTF8, "application/json"));

            string responseString = await res.Content.ReadAsStringAsync();
            HttpStatusCode statusCode = res.StatusCode;
            if (statusCode == HttpStatusCode.Created)
            {
                // Parse the response and log it
                PresentationResponse presentationResponse = PresentationResponse.Parse(responseString);
                _logger.LogTrace($"Request API response: {responseString}");

                // We use in memory cache to keep state about the request. The UI will check the state when calling the presentationResponse method
                var stateData = new StateData()
                {
                    Status = "request_created",
                    Message = "Waiting for the user to open the notification.",
                    Expiry = presentationResponse.Expiry,
                    StateID = request.Callback.State,
                    URL = presentationResponse.URL,
                };

                // Cache the state data with a key based on the state
                // The key is the state value from the callback definition in the request
                _cache.Set(
                            request.Callback.State,
                            stateData.ToString(),
                            DateTimeOffset.Now.AddSeconds(_configuration.GetValue<int>("VerifiedID:CacheExpiresInSeconds", 300)));

                // Return the response as JSON
                return new ContentResult { ContentType = "application/json", Content = stateData.ToString() };
            }
            else
            {
                _logger.LogError($"Error calling Verified ID request API: {responseString}");
                return BadRequest(new { error = "400", error_description = $"Verified ID API error response: {responseString}", request = jsonString });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception: " + ex.Message);
            return BadRequest(new { error = "400", error_description = "Exception: " + ex.Message });
        }
    }

    public PresentationRequest CreatePresentationRequest(AjaxPayload ajaxPayload)
    {

        // Check if the DidAuthority key is set in the appsettings.json file
        if (string.IsNullOrWhiteSpace(_configuration["VerifiedID:DidAuthority"]))
        {
            throw new ArgumentException("Missing the 'VerifiedID:DidAuthority' configuration for Entra ID. Please check the appsettings.json file.");
        }

        PresentationRequest request = new PresentationRequest()
        {
            IncludeQRCode = false,
            Authority = _configuration["VerifiedID:DidAuthority"]!,
            Registration = new RequestRegistration()
            {
                ClientName = _configuration.GetValue("VerifiedID:Presentation:ClientName", "Helpdesk"),
                Purpose = _configuration.GetValue("VerifiedID:Presentation:Purpose", "To prove your identity")
            },
            Callback = new CallbackDefinition()
            {
                Url = $"{GetRequestHostName()}/api/verifier/presentationcallback",
                State = Guid.NewGuid().ToString(),
                Headers = new Dictionary<string, string>() { { "api-key", System.Environment.GetEnvironmentVariable("API-KEY")! } }
            },
            IncludeReceipt = false,
            RequestedCredentials = new List<RequestedCredential>(),
        };


        // Add the credential type to the request
        request.RequestedCredentials.Add(new RequestedCredential()
        {
            Type = _configuration.GetValue("VerifiedID:Presentation:CredentialType", "Helpdesk"),
            AcceptedIssuers = new List<string> { _configuration["VerifiedID:DidAuthority"]! },
            Configuration = new Configuration()
            {
                Validation = new Validation()
                {
                    AllowRevoked = false,
                    ValidateLinkedDomain = true
                }
            }
        });

        // Check if the UpnConstraint key is set in the appsettings.json file is set to true
        if (_configuration.GetValue("VerifiedID:Presentation:UpnConstraint", false))
        {
            // Add the UPN constraint to the request
            request.RequestedCredentials[0].Constraints = new List<Constraint>()
            {
                new Constraint()
                {
                    ClaimName = "revocationId",
                    Values = new List<string> { ajaxPayload.Upn! }
                }
            };
        }
        
        // Add the face check validation if enabled
        if (_configuration.GetValue("VerifiedID:Presentation:UseFaceCheck", false))
        {
            // Receipt is not supported while doing faceCheck
            request.IncludeReceipt = false;

            request.RequestedCredentials[0].Configuration.Validation.FaceCheck = new FaceCheck()
            {
                SourcePhotoClaimName = _configuration.GetValue("VerifiedID:Presentation:SourcePhotoClaimName", "photo"),
                MatchConfidenceThreshold = _configuration.GetValue("VerifiedID:Presentation:MatchConfidenceThreshold", 70)
            };
        }

        return request;
    }

    // Get the host name from the request
    // This is used to create the callback URL for the presentation request
    protected string GetRequestHostName()
    {
        string scheme = "https";
        string originalHost = this.Request.Headers["x-original-host"].ToString() ?? string.Empty;
        string hostname = "";
        if (!string.IsNullOrEmpty(originalHost))
            hostname = string.Format("{0}://{1}", scheme, originalHost);
        else hostname = string.Format("{0}://{1}", scheme, this.Request.Host);
        return hostname;
    }
}
