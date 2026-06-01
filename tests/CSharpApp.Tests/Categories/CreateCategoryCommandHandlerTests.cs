namespace CSharpApp.Tests.Categories;

public class CreateCategoryCommandHandlerTests
{
    private readonly Mock<ICategoriesService> _categoriesServiceMock;
    private readonly CreateCategoryCommandHandler _handler;

    public CreateCategoryCommandHandlerTests()
    {
        _categoriesServiceMock = new Mock<ICategoriesService>();
        _handler = new CreateCategoryCommandHandler(_categoriesServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCreatedCategory()
    {
        // Arrange
        var dto = new CreateCategoryDto
        {
            Name = "New Category",
            Image = "https://example.com/image.jpg"
        };

        var createdCategory = new Category { Id = 5, Name = "New Category" };

        _categoriesServiceMock
            .Setup(s => s.CreateCategory(dto))
            .ReturnsAsync(createdCategory);

        // Act
        var result = await _handler.Handle(new CreateCategoryCommand(dto), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(5);
        result.Name.Should().Be("New Category");
    }

    [Fact]
    public async Task Handle_ShouldCallCreateCategory_Once()
    {
        var dto = new CreateCategoryDto { Name = "Test", Image = "https://example.com/image.jpg" };

        _categoriesServiceMock
            .Setup(s => s.CreateCategory(dto))
            .ReturnsAsync(new Category { Id = 1, Name = "Test" });

        await _handler.Handle(new CreateCategoryCommand(dto), CancellationToken.None);

        _categoriesServiceMock.Verify(s => s.CreateCategory(dto), Times.Once);
    }
}
