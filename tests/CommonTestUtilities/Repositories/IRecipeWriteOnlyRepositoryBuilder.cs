using Moq;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.Recipe;

namespace CommonTestUtilities.Repositories;

public class IRecipeWriteOnlyRepositoryBuilder
{
    private readonly Mock<IRecipeWriteOnlyRepository> _mock;

    public IRecipeWriteOnlyRepositoryBuilder()
    {
        _mock = new Mock<IRecipeWriteOnlyRepository>();
    }

    public IRecipeWriteOnlyRepositoryBuilder DeleteById(Recipe recipe)
    {
        _mock.Setup(repository => repository.DeleteById(recipe.Id, recipe.UserId)).ReturnsAsync(true);

        return this;
    }

    public IRecipeWriteOnlyRepository Build() => _mock.Object;
}
