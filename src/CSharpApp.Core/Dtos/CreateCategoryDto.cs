namespace CSharpApp.Core.Dtos;

public sealed class CreateCategoryDto
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("image")]
    public string? Image { get; set; }
}
