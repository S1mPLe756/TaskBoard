using System.Text.Json.Serialization;

namespace ApiGateway.DTOs;

public class ServiceHealthResponseDto
{
    [JsonPropertyName("serviceName")]
    public string ServiceName { get; set; } = "";
    [JsonPropertyName("status")]
    public string Status { get; set; } = ""; 
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("duration")]
    public string Duration { get; set; } = "";
}