using System.Text.Json.Serialization;

namespace helpdesk_prove_request.Model.Callback;
public class CallbackDomainValidation
{
    /// <summary>
    ///  URL of the domain validation
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; set; }
}
