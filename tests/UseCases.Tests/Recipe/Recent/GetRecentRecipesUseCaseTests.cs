using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Storage;
using MyRecipeBook.Application.UseCases.Recipe.Recent;
using Shouldly;

namespace UseCases.Tests.Recipe.Recent;

public class GetRecentRecipesUseCaseTests
{
    [Theory]
    [InlineData(true, IStorageServiceBuilder.FakeUrl)]
    [InlineData(false, "")]
    public async Task Success(bool hasImage, string expectedUrl)
    {
        var (user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);
        recipe.HasImage = hasImage;

        var useCase = CreateUseCase(user, [recipe]);

        var result = await useCase.Execute();

        result.ShouldNotBeNull();
        result.Recipes.ShouldNotBeNull();
        result.Recipes.Count.ShouldBe(1);
        result.Recipes.ShouldContain(recipeSummary => recipeSummary.Id == recipe.Id && recipeSummary.Title.Equals(recipe.Title));
        result.Recipes.ShouldAllBe(recipeSummary => recipeSummary.ImageUrl.Equals(expectedUrl));
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
        var storageService = IStorageServiceBuilder.Build();

        return new GetRecentRecipesUseCase(loggedUser, repository, storageService);
    }
}
