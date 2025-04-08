using System.Text.Json.Serialization;

namespace helpdesk_prove_request.Model.Presentation;

/// <summary>
/// Configuration - presentation validation configuration
/// </summary>
public class Configuration
{
    /// <summary>
    /// Provides information about how the presented credentials should be validated.
    /// </summary>  
    [JsonPropertyName("validation")]
    public Validation Validation { get; set; }
}