namespace CSharpApp.Application.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(CreateCategoryDto CreateCategoryDto) : IRequest<Category?>;
