namespace CSharpApp.Application.Auth;

public class AuthorizationMessageHandler : DelegatingHandler
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthorizationMessageHandler> _logger;

    public AuthorizationMessageHandler(IAuthService authService,
        ILogger<AuthorizationMessageHandler> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _authService.GetTokenAsync();

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _logger.LogInformation("Authorization header added to request");
        }
        else
        {
            _logger.LogWarning("No token available, request will be sent without Authorization header");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}