namespace CSharpApp.Tests.Products;

public class GetProductQueryHandlerTests
{
    private readonly Mock<IProductsService> _productsServiceMock;
    private readonly GetProductQueryHandler _handler;

    public GetProductQueryHandlerTests()
    {
        _productsServiceMock = new Mock<IProductsService>();
        _handler = new GetProductQueryHandler(_productsServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        var product = new Product { Id = 1, Title = "Product 1", Price = 10 };

        _productsServiceMock
            .Setup(s => s.GetProduct(1))
            .ReturnsAsync(product);

        // Act
        var result = await _handler.Handle(new GetProductQuery(1), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Title.Should().Be("Product 1");
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenProductDoesNotExist()
    {
        _productsServiceMock
            .Setup(s => s.GetProduct(999))
            .ReturnsAsync((Product?)null);

        var result = await _handler.Handle(new GetProductQuery(999), CancellationToken.None);

        result.Should().BeNull();
    }
}
