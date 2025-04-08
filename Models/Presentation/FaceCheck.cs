using System.Text.Json.Serialization;

namespace helpdesk_prove_request.Model.Presentation;

/// <summary>
/// FaceCheck - if to ask for face check and what claim + score you want
/// </summary>
public class FaceCheck
{
    /// <summary>
    /// Mandatory. The name of the claim in the credential that contains the photo. 
    /// </summary>  
    [JsonPropertyName("sourcePhotoClaimName")]
    public string SourcePhotoClaimName { get; set; }

    /// <summary>
    /// Optional. The confidential threshold for a successful check between the photo and the liveness data. 
    /// Must be an integer between 50 and 100. The default is 70.
    /// </summary>  
    [JsonPropertyName("matchConfidenceThreshold	")]
    public int MatchConfidenceThreshold { get; set; }
}