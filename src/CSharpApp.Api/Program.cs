var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Logging.ClearProviders().AddSerilog(logger);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDefaultConfiguration(builder.Configuration);
builder.Services.AddProblemDetails();
builder.Services.AddApiVersioning();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

var versionedEndpointRouteBuilder = app.NewVersionedApi();

versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getproducts", async (IProductsService productsService) =>
    {
        var products = await productsService.GetProducts();
        return products;
    })
    .WithName("GetProducts")
    .HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getproducts/{id}", async (int id, IProductsService productsService) =>
    {
        var product = await productsService.GetProduct(id);
        return product is null ? Results.NotFound() : Results.Ok(product);
    })
    .WithName("GetProduct")
    .HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapPost("api/v{version:apiVersion}/createproduct", async (CreateProductDto createProductDto, IProductsService productsService) =>
    {
        var product = await productsService.CreateProduct(createProductDto);
        return Results.Created($"api/v1/getproducts/{product!.Id}", product);
    })
    .WithName("CreateProduct")
    .HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getcategories", async (ICategoriesService categoriesService) =>
    {
        var categories = await categoriesService.GetCategories();
        return categories;
    })
    .WithName("GetCategories")
    .HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getcategories/{id}", async (int id, ICategoriesService categoriesService) =>
    {
        var category = await categoriesService.GetCategory(id);
        return category is null ? Results.NotFound() : Results.Ok(category);
    })
    .WithName("GetCategory")
    .HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapPost("api/v{version:apiVersion}/createcategory", async (CreateCategoryDto createCategoryDto, ICategoriesService categoriesService) =>
    {
        var category = await categoriesService.CreateCategory(createCategoryDto);
        return Results.Created($"api/v1/getcategories/{category!.Id}", category);
    })
    .WithName("CreateCategory")
    .HasApiVersion(1.0);

app.Run();