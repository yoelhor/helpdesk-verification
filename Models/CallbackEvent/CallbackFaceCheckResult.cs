using System.Text.Json.Serialization;

namespace helpdesk_prove_request.Model.Callback;

public class CallbackFaceCheckResult
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("matchConfidenceScore")]
    public double MatchConfidenceScore { get; set; }
}
