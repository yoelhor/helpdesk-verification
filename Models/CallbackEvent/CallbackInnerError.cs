
using System.Text.Json.Serialization;

namespace helpdesk_prove_request.Model.Callback;

/// <summary>
/// Provide details on what caused the error.
/// </summary>
public class CallbackInnerError
{
    /// <summary>
    /// Contains a standardized code, based on the type of the error
    /// </summary>
    [JsonPropertyName("code")]
    public string Code { get; set; }

    /// <summary>
    /// The internal error message. Contains a detailed message of the error.
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; }

    /// <summary>
    /// contains the field in the request that is causing this error. 
    /// This field is optional and may not be present, depending on the error type.
    /// </summary>
    [JsonPropertyName("target")]
    public string Target { get; set; }
}