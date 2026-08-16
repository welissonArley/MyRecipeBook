using CommonTestUtilities.Entities;
using CommonTestUtilities.Files;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Storage;
using MyRecipeBook.Application.UseCases.User.ChangeProfilePicture;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionsBase;
using Shouldly;
using System.Net;

namespace UseCases.Tests.User.ChangeProfilePicture;

public class ChangeProfilePictureUseCaseTests
{
    [Fact]
    public async Task Success_WhenPng()
    {
        var useCase = CreateUseCase();

        await useCase.Execute(FileBuilder.GetPng()).ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Success_WhenJpeg()
    {
        var useCase = CreateUseCase();

        await useCase.Execute(FileBuilder.GetJpeg()).ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_WhenImageIsBmp()
    {
        var useCase = CreateUseCase();

        var exception = await useCase.Execute(FileBuilder.GetBmp()).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);

        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_ONLY_IMAGES_ACCEPTED);
        });
    }

    [Fact]
    public async Task Error_WhenImageIsTxt()
    {
        var useCase = CreateUseCase();

        var exception = await useCase.Execute(FileBuilder.GetTxt()).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);

        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_ONLY_IMAGES_ACCEPTED);
        });
    }

    private static ChangeProfilePictureUseCase CreateUseCase()
    {
        var (user, _) = UserBuilder.Build();
        var loggedUser = ILoggedUserBuilder.Build(user);
        var storageService = IStorageServiceBuilder.Build();
        var userUpdateOnlyRepository = IUserUpdateOnlyRepositoryBuilder.Build();

        return new ChangeProfilePictureUseCase(loggedUser, storageService, userUpdateOnlyRepository);
    }
}
