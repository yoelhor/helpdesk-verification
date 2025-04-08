using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace helpdesk_prove_request.Model.Ajax;
public class AjaxPayload
{
    [Required]
    [JsonPropertyName("upn")]
    public string Upn { get; set; }

    [JsonPropertyName("phone")]
    public string? Phone { get; set; }
}