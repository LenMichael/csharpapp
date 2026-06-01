namespace CSharpApp.Tests.Categories;

public class GetCategoryQueryHandlerTests
{
    private readonly Mock<ICategoriesService> _categoriesServiceMock;
    private readonly GetCategoryQueryHandler _handler;

    public GetCategoryQueryHandlerTests()
    {
        _categoriesServiceMock = new Mock<ICategoriesService>();
        _handler = new GetCategoryQueryHandler(_categoriesServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCategory_WhenCategoryExists()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "Category 1" };

        _categoriesServiceMock
            .Setup(s => s.GetCategory(1))
            .ReturnsAsync(category);

        // Act
        var result = await _handler.Handle(new GetCategoryQuery(1), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Name.Should().Be("Category 1");
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenCategoryDoesNotExist()
    {
        _categoriesServiceMock
            .Setup(s => s.GetCategory(999))
            .ReturnsAsync((Category?)null);

        var result = await _handler.Handle(new GetCategoryQuery(999), CancellationToken.None);

        result.Should().BeNull();
    }
}
