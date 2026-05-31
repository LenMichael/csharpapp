using System.Net.Http.Json;

namespace CSharpApp.Application.Products;

public class ProductsService : IProductsService
{
    private readonly HttpClient _httpClient;
    private readonly RestApiSettings _restApiSettings;
    private readonly ILogger<ProductsService> _logger;

    public ProductsService(HttpClient httpClient,
        IOptions<RestApiSettings> restApiSettings,
        ILogger<ProductsService> logger)
    {
        _httpClient = httpClient;
        _restApiSettings = restApiSettings.Value;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<Product>> GetProducts()
    {
        _logger.LogInformation("Fetching all products");
        var response = await _httpClient.GetAsync(_restApiSettings.Products);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var res = JsonSerializer.Deserialize<List<Product>>(content);
        _logger.LogInformation("Fetched {Count} products", res!.Count);
        return res!.AsReadOnly();
    }

    public async Task<Product?> GetProduct(int id)
    {
        _logger.LogInformation("Fetching product with id {Id}", id);
        var response = await _httpClient.GetAsync($"{_restApiSettings.Products}/{id}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var product = JsonSerializer.Deserialize<Product>(content);
        _logger.LogInformation("Fetched product: {Title}", product?.Title);
        return product;
    }

    public async Task<Product?> CreateProduct(CreateProductDto createProductDto)
    {
        var response = await _httpClient.PostAsJsonAsync(_restApiSettings.Products, createProductDto);
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("CreateProduct failed: {StatusCode} - {Content}", response.StatusCode, content);
            response.EnsureSuccessStatusCode();
        }
        return JsonSerializer.Deserialize<Product>(content);
    }
}