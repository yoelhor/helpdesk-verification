using System.Text.Json;
using System.Text.Json.Serialization;
using helpdesk_prove_request.Model.Callback;

namespace helpdesk_prove_request.Model;

public class StateData
{
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? URL { get; set; }

    [JsonPropertyName("stateID")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? StateID { get; set; }

    [JsonPropertyName("callback")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Callback { get; set; }

    [JsonPropertyName("expiry")]
    public int Expiry { get; set; }
    
    [JsonPropertyName("subject")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Subject { get; set; }

    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? Type { get; set; }
    
    [JsonPropertyName("claims")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CallbackClaims? Claims { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
    }

        /// <summary>
    /// Deserialize a JSON string into CallbackData object
    /// </summary>
    /// <param name="JsonString">The JSON string to be loaded</param>
    /// <returns></returns>
    public static StateData Parse(string JsonString)
    {
        var result = JsonSerializer.Deserialize<StateData>(JsonString);
        if (result == null)
        {
            throw new InvalidOperationException("Deserialization returned null.");
        }
        return result;
    }
}