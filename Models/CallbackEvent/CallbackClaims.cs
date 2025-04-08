using System.Text.Json.Serialization;

namespace helpdesk_prove_request.Model.Callback;


public class CallbackClaims
{

    /// <summary>
    /// First name
    /// </summary>
    [JsonPropertyName("firstName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string FirstName { get; set; }

    /// <summary>
    /// Last name
    /// </summary>
    [JsonPropertyName("lastName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string LastName { get; set; }

    /// <summary>
    /// ID
    /// </summary>
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string ID { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("sum")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Sum { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("displayName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string DisplayName { get; set; }
}