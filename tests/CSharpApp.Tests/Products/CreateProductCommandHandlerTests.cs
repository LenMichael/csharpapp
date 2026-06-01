namespace CSharpApp.Tests.Products;

public class CreateProductCommandHandlerTests
{
    private readonly Mock<IProductsService> _productsServiceMock;
    private readonly CreateProductCommandHandler _handler;

    public CreateProductCommandHandlerTests()
    {
        _productsServiceMock = new Mock<IProductsService>();
        _handler = new CreateProductCommandHandler(_productsServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCreatedProduct()
    {
        // Arrange
        var dto = new CreateProductDto
        {
            Title = "New Product",
            Price = 50,
            Description = "A description",
            CategoryId = 1,
            Images = ["https://example.com/image.jpg"]
        };

        var createdProduct = new Product { Id = 10, Title = "New Product", Price = 50 };

        _productsServiceMock
            .Setup(s => s.CreateProduct(dto))
            .ReturnsAsync(createdProduct);

        // Act
        var result = await _handler.Handle(new CreateProductCommand(dto), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(10);
        result.Title.Should().Be("New Product");
    }

    [Fact]
    public async Task Handle_ShouldCallCreateProduct_Once()
    {
        var dto = new CreateProductDto { Title = "Test", Price = 10, CategoryId = 1 };

        _productsServiceMock
            .Setup(s => s.CreateProduct(dto))
            .ReturnsAsync(new Product { Id = 1, Title = "Test" });

        await _handler.Handle(new CreateProductCommand(dto), CancellationToken.None);

        _productsServiceMock.Verify(s => s.CreateProduct(dto), Times.Once);
    }
}
