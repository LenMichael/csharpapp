namespace CSharpApp.Core.Dtos;

public sealed class LoginDto
{
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("password")]
    public string? Password { get; set; }
}