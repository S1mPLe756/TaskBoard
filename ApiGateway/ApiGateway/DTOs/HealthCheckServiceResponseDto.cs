using System.Text.Json.Serialization;

namespace ApiGateway.DTOs;

public class HealthCheckServiceResponseDto
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = "";
    [JsonPropertyName("services")]
    public List<ServiceHealthResponseDto> Services { get; set; } = new();
}