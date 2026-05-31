namespace CSharpApp.Application.Auth;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly RestApiSettings _restApiSettings;
    private readonly ILogger<AuthService> _logger;

    public AuthService(HttpClient httpClient,
        IOptions<RestApiSettings> restApiSettings,
        ILogger<AuthService> logger)
    {
        _httpClient = httpClient;
        _restApiSettings = restApiSettings.Value;
        _logger = logger;
    }

    public async Task<string?> GetTokenAsync()
    {
        _logger.LogInformation("Requesting JWT token");

        var loginDto = new LoginDto
        {
            Email = _restApiSettings.Username,
            Password = _restApiSettings.Password
        };

        var response = await _httpClient.PostAsJsonAsync(_restApiSettings.Auth, loginDto);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Authentication failed: {StatusCode} - {Content}", response.StatusCode, content);
            return null;
        }

        var token = JsonSerializer.Deserialize<TokenDto>(content);
        _logger.LogInformation("JWT token acquired successfully");
        return token?.AccessToken;
    }
}