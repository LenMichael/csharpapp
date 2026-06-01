namespace CSharpApp.Application.Products.Commands.CreateProduct;

public record CreateProductCommand(CreateProductDto CreateProductDto) : IRequest<Product?>;
