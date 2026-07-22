using Moq;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.Recipe;

namespace CommonTestUtilities.Repositories;

public class IRecipeUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IRecipeUpdateOnlyRepository> _mock;

    public IRecipeUpdateOnlyRepositoryBuilder()
    {
        _mock = new Mock<IRecipeUpdateOnlyRepository>();
    }

    public IRecipeUpdateOnlyRepositoryBuilder GetById(Recipe recipe)
    {
        _mock.Setup(repository => repository.GetById(recipe.Id, recipe.UserId)).ReturnsAsync(recipe);

        return this;
    }

    public IRecipeUpdateOnlyRepository Build() => _mock.Object;
}
