namespace ApiGateway;

public class AddUserIdDelegatingHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Headers.Authorization != null)
        {
            var token = request.Headers.Authorization.Parameter;
            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var userId = jwt.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (!string.IsNullOrEmpty(userId))
                request.Headers.Add("X-User-Id", userId);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
