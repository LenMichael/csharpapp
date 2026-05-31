namespace CSharpApp.Application.Categories;

public class CategoriesService : ICategoriesService
{
    private readonly HttpClient _httpClient;
    private readonly RestApiSettings _restApiSettings;
    private readonly ILogger<CategoriesService> _logger;

    public CategoriesService(HttpClient httpClient,
        IOptions<RestApiSettings> restApiSettings,
        ILogger<CategoriesService> logger)
    {
        _httpClient = httpClient;
        _restApiSettings = restApiSettings.Value;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<Category>> GetCategories()
    {
        _logger.LogInformation("Fetching all categories");
        var response = await _httpClient.GetAsync(_restApiSettings.Categories);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var res = JsonSerializer.Deserialize<List<Category>>(content);
        _logger.LogInformation("Fetched {Count} categories", res!.Count);
        return res!.AsReadOnly();
    }

    public async Task<Category?> GetCategory(int id)
    {
        _logger.LogInformation("Fetching category with id {Id}", id);
        var response = await _httpClient.GetAsync($"{_restApiSettings.Categories}/{id}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var category = JsonSerializer.Deserialize<Category>(content);
        _logger.LogInformation("Fetched category: {Name}", category?.Name);
        return category;
    }

    public async Task<Category?> CreateCategory(CreateCategoryDto createCategoryDto)
    {
        var response = await _httpClient.PostAsJsonAsync(_restApiSettings.Categories, createCategoryDto);
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("CreateCategory failed: {StatusCode} - {Content}", response.StatusCode, content);
            response.EnsureSuccessStatusCode();
        }
        return JsonSerializer.Deserialize<Category>(content);
    }
}
