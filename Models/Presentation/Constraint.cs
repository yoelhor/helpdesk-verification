using System.Text.Json.Serialization;

namespace helpdesk_prove_request.Model.Presentation;

public class Constraint
{
    /// <summary>
    /// Mandatory. Name of the claim for the constraint. This is the claim name in the verifiable credential
    /// </summary>  
    [JsonPropertyName("claimName")]
    public string ClaimName { get; set; }

    /// <summary>
    /// Set of values that should match the claim value. If you specify multiple values, like ["red", "green", "blue"] 
    /// it is a match if the claim value in the credential has any of the values in the collection.
    /// </summary>  
    [JsonPropertyName("values")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string> Values { get; set; }

    /// <summary>
    /// The constraint evaluates to true if the claim value contains the specified value.
    /// </summary>  
    [JsonPropertyName("contains")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Contains { get; set; }

    /// <summary>
    /// The constraint evaluates to true if the claim value starts with the specified value.
    /// </summary>  
    [JsonPropertyName("startsWith")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string StartsWith { get; set; }
}