using Moq;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.Recipe;

namespace CommonTestUtilities.Repositories;
public class RecipeUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IRecipeUpdateOnlyRepository> _repository;

    public RecipeUpdateOnlyRepositoryBuilder() => _repository = new Mock<IRecipeUpdateOnlyRepository>();

    public RecipeUpdateOnlyRepositoryBuilder GetById(User user, Recipe? recipe)
    {
        if (recipe is not null)
            _repository.Setup(repository => repository.GetById(user, recipe.Id)).ReturnsAsync(recipe);

        return this;
    }

    public IRecipeUpdateOnlyRepository Build() => _repository.Object;
}
