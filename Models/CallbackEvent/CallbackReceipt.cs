using System.Text.Json.Serialization;

namespace helpdesk_prove_request.Model.Callback;


/// <summary>
/// CallbackReceipt - returned when VC presentation is verified. The id_token contains the full VC id_token
/// the state is not to be confused with the CallbackData.state and is something internal to the VC Client API
/// </summary>
public class CallbackReceipt
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("id_token")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string IdToken { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("vp_token")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string VpToken { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("state")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string State { get; set; }
}
