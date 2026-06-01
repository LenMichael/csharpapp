namespace CSharpApp.Application.Categories.Queries.GetCategory;

public record GetCategoryQuery(int Id) : IRequest<Category?>;
