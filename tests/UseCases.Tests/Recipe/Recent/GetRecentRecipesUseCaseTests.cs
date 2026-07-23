using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using MyRecipeBook.Application.UseCases.Recipe.Recent;
using Shouldly;

namespace UseCases.Tests.Recipe.Recent;

public class GetRecentRecipesUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);

        var useCase = CreateUseCase(user, [recipe]);

        var result = await useCase.Execute();

        result.ShouldNotBeNull();
        result.Recipes.ShouldNotBeNull();
        result.Recipes.Count.ShouldBe(1);
        result.Recipes.ShouldContain(recipeSummary => recipeSummary.Id == recipe.Id && recipeSummary.Title.Equals(recipe.Title));
    }

    [Fact]
    public async Task Success_WhenThereAreNoRecipes()
    {
        var (user, _) = UserBuilder.Build();

        var useCase = CreateUseCase(user, []);

        var result = await useCase.Execute();

        result.ShouldNotBeNull();
        result.Recipes.ShouldNotBeNull();
        result.Recipes.ShouldBeEmpty();
    }

    private static GetRecentRecipesUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user, IList<MyRecipeBook.Domain.Entities.Recipe> recipes)
    {
        var loggedUser = ILoggedUserBuilder.Build(user);
        var repository = new IRecipeReadOnlyRepositoryBuilder().GetRecentRecipes(user, recipes).Build();

        return new GetRecentRecipesUseCase(loggedUser, repository);
    }
}
