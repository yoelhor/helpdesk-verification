using System.Text.Json.Serialization;

namespace helpdesk_prove_request.Model.Callback;

/// <summary>
/// If there's an error with the request, an error responses is returned
/// </summary>
public class CallbackError
{
    /// <summary>
    /// The return error code.
    /// </summary>
    [JsonPropertyName("code")]
    public string Code { get; set; }

    /// <summary>
    /// The error message.
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; }
}