namespace CSharpApp.Application.Categories.Queries.GetCategory;

public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, Category?>
{
    private readonly ICategoriesService _categoriesService;

    public GetCategoryQueryHandler(ICategoriesService categoriesService)
    {
        _categoriesService = categoriesService;
    }

    public async Task<Category?> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        return await _categoriesService.GetCategory(request.Id);
    }
}
