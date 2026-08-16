using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Storage;
using MyRecipeBook.Application.UseCases.Recipe.Filter;
using MyRecipeBook.Communication.Requests;
using Shouldly;

namespace UseCases.Tests.Recipe.Filter;

public class FilterRecipesUseCaseTests
{
    [Theory]
    [InlineData(true, IStorageServiceBuilder.FakeUrl)]
    [InlineData(false, "")]
    public async Task Success_WhenRequestIsNull(bool hasImage, string expectedUrl)
    {
        var (user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);
        recipe.HasImage = hasImage;

        var useCase = CreateUseCase(user, [recipe]);

        var result = await useCase.Execute(request: null);

        result.ShouldNotBeNull();
        result.Recipes.ShouldNotBeNull();
        result.Recipes.Count.ShouldBe(1);
        result.Recipes.ShouldContain(recipeSummary => recipeSummary.Id == recipe.Id && recipeSummary.Title.Equals(recipe.Title));
        result.Recipes.ShouldAllBe(recipeSummary => recipeSummary.ImageUrl.Equals(expectedUrl));
    }

    [Theory]
    [InlineData(true, IStorageServiceBuilder.FakeUrl)]
    [InlineData(false, "")]
    public async Task Success(bool hasImage, string expectedUrl)
    {
        var (user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);
        recipe.HasImage = hasImage;

        var useCase = CreateUseCase(user, [recipe]);

        var request = new RequestFilterRecipesJson();

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Recipes.ShouldNotBeNull();
        result.Recipes.Count.ShouldBe(1);
        result.Recipes.ShouldContain(recipeSummary => recipeSummary.Id == recipe.Id && recipeSummary.Title.Equals(recipe.Title));
        result.Recipes.ShouldAllBe(recipeSummary => recipeSummary.ImageUrl.Equals(expectedUrl));
    }

    private static FilterRecipesUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user, IList<MyRecipeBook.Domain.Entities.Recipe> recipes)
    {
        var loggedUser = ILoggedUserBuilder.Build(user);
        var repository = new IRecipeReadOnlyRepositoryBuilder().FilterRecipes(user, recipes).Build();
        var storageService = IStorageServiceBuilder.Build();

        return new FilterRecipesUseCase(loggedUser, repository, storageService);
    }
}
