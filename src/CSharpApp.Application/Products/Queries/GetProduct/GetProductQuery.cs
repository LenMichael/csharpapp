namespace CSharpApp.Application.Products.Queries.GetProduct;

public record GetProductQuery(int Id) : IRequest<Product?>;
