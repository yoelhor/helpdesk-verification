
using System.Text.Json.Serialization;

namespace helpdesk_prove_request.Model.Presentation;

/// <summary>
/// Registration - used in both issuance and presentation to give the app a display name
/// </summary>
public class RequestRegistration
{

    /// <summary>
    /// A display name of the verifier of the verifiable credential. This name will be presented to the user in the authenticator app.
    /// </summary>  
    [JsonPropertyName("clientName")]
    public string ClientName { get; set; }

    /// <summary>
    /// Optional. A string that is displayed to inform the user why the verifiable credentials are being requested.
    /// </summary>  
    [JsonPropertyName("purpose")]
    public string Purpose { get; set; }

    /// <summary>
    /// Optional. A URL for a logotype of the verifier. This is not used by the Authenticator app.
    /// </summary>  
    [JsonPropertyName("logoUrl")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string LogoUrl { get; set; }

    /// <summary>
    /// Optional. A URL to the terms of service for the verifier. This is not used by the Authenticator app.
    /// </summary>  
    [JsonPropertyName("termsOfServiceUrl")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string TermsOfServiceUrl { get; set; }

}