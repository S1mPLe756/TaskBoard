namespace ApiGateway.DTOs;

public class HealthCheckResponse
{
    public string Status { get; set; } = "";
    public List<ServiceHealth> Services { get; set; } = new();
}