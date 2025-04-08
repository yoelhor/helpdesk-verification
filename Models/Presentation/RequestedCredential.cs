using System.Text.Json.Serialization;

namespace helpdesk_prove_request.Model.Presentation;

/// <summary>
/// Presentation can involve asking for multiple VCs
/// </summary>
public class RequestedCredential
{
    /// <summary>
    /// The verifiable credential type. 
    // The type must match the type as defined in the issuer verifiable credential manifest (for example, VerifiedCredentialExpert).
    /// </summary>  
    [JsonPropertyName("type")]
    public string Type { get; set; }

    /// <summary>
    /// Optional. Provide information about the purpose of requesting this verifiable credential. This is not used by the Authenticator app.
    /// </summary>  
    [JsonPropertyName("purpose")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Purpose { get; set; }

    /// <summary>
    /// Optional. A collection of issuers' DIDs that could issue the type of verifiable credential that subjects can present. 
    /// </summary>  
    [JsonPropertyName("acceptedIssuers")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string> AcceptedIssuers { get; set; }

    /// <summary>
    /// Optional settings for presentation validation.
    /// </summary>  
    [JsonPropertyName("configuration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Configuration Configuration { get; set; }

    /// <summary>
    /// Optional. Collection of claims constraints.
    /// </summary>  
    [JsonPropertyName("constraints")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<Constraint> Constraints { get; set; }
}