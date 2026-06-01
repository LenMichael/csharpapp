namespace CSharpApp.Tests.Categories;

public class GetCategoriesQueryHandlerTests
{
    private readonly Mock<ICategoriesService> _categoriesServiceMock;
    private readonly GetCategoriesQueryHandler _handler;

    public GetCategoriesQueryHandlerTests()
    {
        _categoriesServiceMock = new Mock<ICategoriesService>();
        _handler = new GetCategoriesQueryHandler(_categoriesServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAllCategories()
    {
        // Arrange
        var categories = new List<Category>
        {
            new() { Id = 1, Name = "Category 1" },
            new() { Id = 2, Name = "Category 2" }
        }.AsReadOnly();

        _categoriesServiceMock
            .Setup(s => s.GetCategories())
            .ReturnsAsync(categories);

        // Act
        var result = await _handler.Handle(new GetCategoriesQuery(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(categories);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoCategories()
    {
        _categoriesServiceMock
            .Setup(s => s.GetCategories())
            .ReturnsAsync(new List<Category>().AsReadOnly());

        var result = await _handler.Handle(new GetCategoriesQuery(), CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
