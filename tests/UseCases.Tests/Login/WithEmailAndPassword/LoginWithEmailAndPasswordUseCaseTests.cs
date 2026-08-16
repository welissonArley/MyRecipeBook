using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using CommonTestUtilities.Storage;
using MyRecipeBook.Application.UseCases.Login.WithEmailAndPassword;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionsBase;
using Shouldly;
using System.Net;

namespace UseCases.Tests.Login.WithEmailAndPassword;

public class LoginWithEmailAndPasswordUseCaseTests
{
    [Theory]
    [InlineData(true, IStorageServiceBuilder.FakeUrl)]
    [InlineData(false, "")]
    public async Task Success(bool hasImage, string expectedUrl)
    {
        var (user, _) = UserBuilder.Build();
        user.HasImage = hasImage;

        var request = RequestLoginJsonBuilder.Build();
        request.Email = user.Email;

        var useCase = CreateUseCase(request.Password, user);

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Tokens.ShouldNotBeNull();
        result.Name.ShouldBe(user.Name);
        result.Tokens.AccessToken.ShouldNotBeNullOrEmpty();
        result.Tokens.RefreshToken.ShouldBeNullOrEmpty();
        result.ImageUrl.ShouldBe(expectedUrl);
    }

    [Fact]
    public async Task ShouldThrowException_WhenUserDontExist()
    {
        var request = RequestLoginJsonBuilder.Build();

        var useCase = CreateUseCase();

        var exception = await useCase.Execute(request).ShouldThrowAsync<InvalidLoginException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.Unauthorized);

        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_LOGIN_INVALID);
        });
    }

    [Fact]
    public async Task ShouldThrowException_WhenPasswordIsIncorrect()
    {
        var (user, _) = UserBuilder.Build();
        var request = RequestLoginJsonBuilder.Build();
        request.Email = user.Email;

        var useCase = CreateUseCase(user: user);

        var exception = await useCase.Execute(request).ShouldThrowAsync<InvalidLoginException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.Unauthorized);

        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_LOGIN_INVALID);
        });
    }

    private LoginWithEmailAndPasswordUseCase CreateUseCase(string? password = null, MyRecipeBook.Domain.Entities.User? user = null)
    {
        var storageService = IStorageServiceBuilder.Build();
        var accessTokenGenerator = IAccessTokenGeneratorBuilder.Build();
        var passwordHasherBuilder = new IPasswordHasherBuilder();
        var userReadOnlyRepositoryBuilder = new IUserReadOnlyRepositoryBuilder();
        if(user is not null)
            userReadOnlyRepositoryBuilder.GetByEmail(user);

        if(password.IsNotEmpty())
            passwordHasherBuilder.VerifyPassword(password);

        return new LoginWithEmailAndPasswordUseCase(passwordHasherBuilder.Build(), userReadOnlyRepositoryBuilder.Build(), accessTokenGenerator, storageService);
    }
}
