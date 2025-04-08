using System.Net;
using helpdesk_prove_request.Model;
using helpdesk_prove_request.Model.Callback;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace helpdesk_prove_request.Controllers;
public class RequestStatusController : Controller
{
    private enum RequestType
    {
        Unknown,
        Presentation,
        Issuance,
        Selfie
    };
    protected IMemoryCache _cache;
    protected readonly ILogger<RequestStatusController> _log;
    private IConfiguration _configuration;

    public RequestStatusController(IConfiguration configuration, IMemoryCache memoryCache, ILogger<RequestStatusController> log)
    {
        _configuration = configuration;
        _cache = memoryCache;
        _log = log;
    }

    [AllowAnonymous]
    [HttpGet("/api/request-status")]
    public ActionResult RequestStatus()
    {
        // Get the request ID from the query string
        string? state = this.Request.Query["id"];

        // Check if the request ID is present if not return an error
        if (string.IsNullOrEmpty(state))
        {
            return BadRequest(new { error = "400", error_description = "Missing argument 'id'" });
        }

        if (_cache.TryGetValue(state, out string? requestState))
        {
            // Return the cached state data in the HTTP response
            return new ContentResult { ContentType = "application/json", Content = requestState };
        }

        // If the request ID is not found in the cache, return an error
        return NotFound(new { error = "404", error_description = "Request ID not found" });
    }
}
