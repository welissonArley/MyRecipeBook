using Moq;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.Recipe;

namespace CommonTestUtilities.Repositories;

public class IRecipeReadOnlyRepositoryBuilder
{
    private readonly Mock<IRecipeReadOnlyRepository> _mock;

    public IRecipeReadOnlyRepositoryBuilder()
    {
        _mock = new Mock<IRecipeReadOnlyRepository>();
    }

    public IRecipeReadOnlyRepositoryBuilder GetById(Recipe recipe)
    {
        _mock.Setup(repository => repository.GetById(recipe.Id, recipe.UserId)).ReturnsAsync(recipe);

        return this;
    }

    public IRecipeReadOnlyRepositoryBuilder GetRecentRecipes(User user, IList<Recipe> recipes)
    {
        _mock.Setup(repository => repository.GetRecentRecipes(user.Id)).ReturnsAsync(recipes);

        return this;
    }

    public IRecipeReadOnlyRepository Build() => _mock.Object;
}
