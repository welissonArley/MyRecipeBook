using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Storage;
using MyRecipeBook.Application.UseCases.Recipe.DeleteById;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionsBase;
using Shouldly;
using System.Net;

namespace UseCases.Tests.Recipe.DeleteById;

public class DeleteRecipeByIdUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);

        var useCase = CreateUseCase(recipe, user);

        await useCase.Execute(recipe.Id).ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenRecipeNotFound()
    {
        var (user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);

        var useCase = CreateUseCase(recipe, user);

        var exception = await useCase.Execute(Guid.CreateVersion7()).ShouldThrowAsync<NotFoundException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.NotFound);

        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_RECIPE_NOT_FOUND);
        });
    }

    private static DeleteRecipeByIdUseCase CreateUseCase(MyRecipeBook.Domain.Entities.Recipe recipe, MyRecipeBook.Domain.Entities.User user)
    {
        var loggedUser = ILoggedUserBuilder.Build(user);
        var repository = new IRecipeWriteOnlyRepositoryBuilder().DeleteById(recipe).Build();
        var storageService = IStorageServiceBuilder.Build();

        return new DeleteRecipeByIdUseCase(repository, loggedUser, storageService);
    }
}
