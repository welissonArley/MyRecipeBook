using CommonTestUtilities.Entities;
using CommonTestUtilities.Files;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Storage;
using MyRecipeBook.Application.UseCases.Recipe.ChangeIllustration;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionsBase;
using Shouldly;
using System.Net;

namespace UseCases.Tests.Recipe.ChangeIllustration;

public class ChangeIllustrationUseCaseTests
{
    [Fact]
    public async Task Success_WhenPng()
    {
        var (user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);

        var useCase = CreateUseCase(user, recipe);

        await useCase.Execute(recipe.Id, FileBuilder.GetPng()).ShouldNotThrowAsync();

        recipe.HasImage.ShouldBeTrue();
    }

    [Fact]
    public async Task Success_WhenJpeg()
    {
        var (user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);

        var useCase = CreateUseCase(user, recipe);

        await useCase.Execute(recipe.Id, FileBuilder.GetJpeg()).ShouldNotThrowAsync();

        recipe.HasImage.ShouldBeTrue();
    }

    [Fact]
    public async Task Error_WhenImageIsBmp()
    {
        var (user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);

        var useCase = CreateUseCase(user, recipe);

        var exception = await useCase.Execute(recipe.Id, FileBuilder.GetBmp()).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);

        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_ONLY_IMAGES_ACCEPTED);
        });

        recipe.HasImage.ShouldBeFalse();
    }

    [Fact]
    public async Task Error_WhenImageIsTxt()
    {
        var (user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);

        var useCase = CreateUseCase(user, recipe);

        var exception = await useCase.Execute(recipe.Id, FileBuilder.GetTxt()).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);

        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_ONLY_IMAGES_ACCEPTED);
        });

        recipe.HasImage.ShouldBeFalse();
    }

    [Fact]
    public async Task Error_WhenRecipeNotFound()
    {
        var (user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);

        var useCase = CreateUseCase(user, recipe);

        var exception = await useCase.Execute(Guid.CreateVersion7(), FileBuilder.GetPng()).ShouldThrowAsync<NotFoundException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.NotFound);

        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_RECIPE_NOT_FOUND);
        });

        recipe.HasImage.ShouldBeFalse();
    }

    private static ChangeIllustrationUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user, MyRecipeBook.Domain.Entities.Recipe recipe)
    {
        var loggedUser = ILoggedUserBuilder.Build(user);
        var storageService = IStorageServiceBuilder.Build();
        var repository = new IRecipeUpdateOnlyRepositoryBuilder().GetById(recipe).Build();
        var unitOfWork = IUnitOfWorkBuilder.Build();

        return new ChangeIllustrationUseCase(loggedUser, storageService, repository, unitOfWork);
    }
}
