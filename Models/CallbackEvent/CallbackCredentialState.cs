using System.Text.Json.Serialization;

namespace helpdesk_prove_request.Model.Callback;


public class CallbackCredentialState
{

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("revocationStatus")]
    public string RevocationStatus { get; set; }


    [JsonIgnore]
    public bool isValid { get { return RevocationStatus == "VALID"; } }
}
