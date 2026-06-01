var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Logging.ClearProviders().AddSerilog(logger);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDefaultConfiguration(builder.Configuration);
builder.Services.AddProblemDetails();
builder.Services.AddApiVersioning();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(GetProductsQueryHandler).Assembly));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.UseMiddleware<PerformanceMiddleware>();

var versionedEndpointRouteBuilder = app.NewVersionedApi();

versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getproducts", async (IMediator mediator) =>
    {
        var products = await mediator.Send(new GetProductsQuery());
        return products;
    })
    .WithName("GetProducts")
    .HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getproducts/{id}", async (int id, IMediator mediator) =>
    {
        var product = await mediator.Send(new GetProductQuery(id));
        return product is null ? Results.NotFound() : Results.Ok(product);
    })
    .WithName("GetProduct")
    .HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapPost("api/v{version:apiVersion}/createproduct", async (CreateProductDto createProductDto, IMediator mediator) =>
    {
        var product = await mediator.Send(new CreateProductCommand(createProductDto));
        return Results.Created($"api/v1/getproducts/{product!.Id}", product);
    })
    .WithName("CreateProduct")
    .HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getcategories", async (IMediator mediator) =>
    {
        var categories = await mediator.Send(new GetCategoriesQuery());
        return categories;
    })
    .WithName("GetCategories")
    .HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getcategories/{id}", async (int id, IMediator mediator) =>
    {
        var category = await mediator.Send(new GetCategoryQuery(id));
        return category is null ? Results.NotFound() : Results.Ok(category);
    })
    .WithName("GetCategory")
    .HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapPost("api/v{version:apiVersion}/createcategory", async (CreateCategoryDto createCategoryDto, IMediator mediator) =>
    {
        var category = await mediator.Send(new CreateCategoryCommand(createCategoryDto));
        return Results.Created($"api/v1/getcategories/{category!.Id}", category);
    })
    .WithName("CreateCategory")
    .HasApiVersion(1.0);

app.Run();