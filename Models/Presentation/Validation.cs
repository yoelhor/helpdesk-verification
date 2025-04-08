using System.Text.Json.Serialization;

namespace helpdesk_prove_request.Model.Presentation;

/// <summary>
/// Validation - presentation validation configuration
/// </summary>
public class Validation
{
    /// <summary>
    /// Optional. Determines if a revoked credential should be accepted. Default is false (it shouldn't be accepted).
    /// </summary>  
    [JsonPropertyName("allowRevoked")]
    public bool AllowRevoked { get; set; } // default false

    /// <summary>
    /// Optional. Determines if the linked domain should be validated. Default is false. 
    /// Setting this flag to false means you as a Relying Party application accept credentials from an unverified linked domain. 
    /// Setting this flag to true means the linked domain will be validated and only verified domains will be accepted.
    /// </summary>  
    [JsonPropertyName("validateLinkedDomain")]
    public bool ValidateLinkedDomain { get; set; } // default false

    /// <summary>
    /// Optional. Allows requesting a liveness check during presentation.
    /// </summary>  
    [JsonPropertyName("faceCheck")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public FaceCheck FaceCheck { get; set; }
}