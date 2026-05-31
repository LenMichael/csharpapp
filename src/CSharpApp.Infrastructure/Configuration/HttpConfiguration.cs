namespace CSharpApp.Infrastructure.Configuration;

public static class HttpConfiguration
{
    public static IServiceCollection AddHttpConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var restApiSettings = configuration.GetSection(nameof(RestApiSettings)).Get<RestApiSettings>();
        var httpClientSettings = configuration.GetSection(nameof(HttpClientSettings)).Get<HttpClientSettings>();

        services.AddHttpClient<IAuthService, AuthService>(client =>
        {
            client.BaseAddress = new Uri(restApiSettings!.BaseUrl!);
        })
        .SetHandlerLifetime(TimeSpan.FromMinutes(httpClientSettings!.LifeTime));

        services.AddTransient<AuthorizationMessageHandler>();

        services.AddHttpClient<IProductsService, ProductsService>(client =>
        {
            client.BaseAddress = new Uri(restApiSettings!.BaseUrl!);
        })
        .SetHandlerLifetime(TimeSpan.FromMinutes(httpClientSettings!.LifeTime))
        .AddHttpMessageHandler<AuthorizationMessageHandler>();

        services.AddHttpClient<ICategoriesService, CategoriesService>(client =>
        {
            client.BaseAddress = new Uri(restApiSettings!.BaseUrl!);
        })
        .SetHandlerLifetime(TimeSpan.FromMinutes(httpClientSettings!.LifeTime))
        .AddHttpMessageHandler<AuthorizationMessageHandler>();

        return services;
    }
}