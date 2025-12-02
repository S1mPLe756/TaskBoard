namespace ApiGateway.DTOs;

public class ServiceHealth
{
    public string ServiceName { get; set; } = "";
    public bool IsHealthy { get; set; } = false;
    public string? Message { get; set; }
}