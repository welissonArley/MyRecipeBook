using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe.GetById;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Xunit;

namespace UseCases.Test.Recipe.GetById;
public class GetRecipeByIdUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);

        var useCase = CreateUseCase(user, recipe);

        var result = await useCase.Execute(recipe.Id);

        result.Should().NotBeNull();
        result.Id.Should().NotBeNullOrWhiteSpace();
        result.Title.Should().Be(recipe.Title);
    }

    [Fact]
    public async Task Error_Recipe_NotFound()
    {
        (var user, _) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => { await useCase.Execute(recipeId: 1000); };

        (await act.Should().ThrowAsync<NotFoundException>())
            .Where(e => e.Message.Equals(ResourceMessagesException.RECIPE_NOT_FOUND));
    }

    private static GetRecipeByIdUseCase CreateUseCase(
        MyRecipeBook.Domain.Entities.User user,
        MyRecipeBook.Domain.Entities.Recipe? recipe = null)
    {
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new RecipeReadOnlyRepositoryBuilder().GetById(user, recipe).Build();

        return new GetRecipeByIdUseCase(mapper, loggedUser, repository);
    }
}
