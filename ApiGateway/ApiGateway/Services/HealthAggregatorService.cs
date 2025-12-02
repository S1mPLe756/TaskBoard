using ApiGateway.DTOs;

namespace ApiGateway.Services;

public class HealthAggregatorService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HealthAggregatorService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    private readonly Dictionary<string, string> _services = new()
    {
        { "AuthService", "http://localhost:5101/api/v1/health" },
        { "UserService", "http://localhost:5103/api/v1/health" },
        { "BoardService", "http://localhost:5104/api/v1/health" },
        { "NotificationService", "http://localhost:5105/api/v1/health" }
    };

    public async Task<HealthCheckResponse> CheckHealthAsync()
    {
        var response = new HealthCheckResponse();
        var client = _httpClientFactory.CreateClient();

        foreach (var svc in _services)
        {
            var svcHealth = new ServiceHealth { ServiceName = svc.Key };

            try
            {
                var httpResponse = await client.GetAsync(svc.Value);
                var content = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    var result = System.Text.Json.JsonSerializer.Deserialize<HealthCheckServiceResponseDto>(content);
                    svcHealth.IsHealthy = result?.Status.Equals("Healthy", StringComparison.OrdinalIgnoreCase) ?? false;
                    svcHealth.Message = string.Join("; ",
                        result?.Services
                            .Where(s => !s.Status.Equals("Healthy", StringComparison.OrdinalIgnoreCase))
                            .Select(s => $"{s.ServiceName}: {s.Description}") ?? Array.Empty<string>());
                }
                else
                {
                    svcHealth.IsHealthy = false;
                    svcHealth.Message = !string.IsNullOrWhiteSpace(content)
                        ? content
                        : $"Service returned {(int)httpResponse.StatusCode} ({httpResponse.ReasonPhrase})";
                }
            }
            catch (HttpRequestException ex)
            {
                svcHealth.IsHealthy = false;
                svcHealth.Message = $"Service unreachable: {ex.Message}";
            }
            catch (Exception ex)
            {
                svcHealth.IsHealthy = false;
                svcHealth.Message = $"Unknown error: {ex.Message}";
            }

            response.Services.Add(svcHealth);
        }

        response.Status = response.Services.All(s => s.IsHealthy) ? "Healthy" : "Unhealthy";

        return response;
    }
}