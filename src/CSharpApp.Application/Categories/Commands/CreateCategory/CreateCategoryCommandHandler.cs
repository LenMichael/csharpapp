namespace CSharpApp.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Category?>
{
    private readonly ICategoriesService _categoriesService;

    public CreateCategoryCommandHandler(ICategoriesService categoriesService)
    {
        _categoriesService = categoriesService;
    }

    public async Task<Category?> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        return await _categoriesService.CreateCategory(request.CreateCategoryDto);
    }
}
