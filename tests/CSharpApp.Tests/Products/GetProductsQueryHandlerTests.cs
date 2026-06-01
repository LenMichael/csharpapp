namespace CSharpApp.Tests.Products;

public class GetProductsQueryHandlerTests
{
    private readonly Mock<IProductsService> _productsServiceMock;
    private readonly GetProductsQueryHandler _handler;

    public GetProductsQueryHandlerTests()
    {
        _productsServiceMock = new Mock<IProductsService>();
        _handler = new GetProductsQueryHandler(_productsServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAllProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Id = 1, Title = "Product 1", Price = 10 },
            new() { Id = 2, Title = "Product 2", Price = 20 }
        }.AsReadOnly();

        _productsServiceMock
            .Setup(s => s.GetProducts())
            .ReturnsAsync(products);

        // Act
        var result = await _handler.Handle(new GetProductsQuery(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(products);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoProducts()
    {
        _productsServiceMock
            .Setup(s => s.GetProducts())
            .ReturnsAsync(new List<Product>().AsReadOnly());

        var result = await _handler.Handle(new GetProductsQuery(), CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
