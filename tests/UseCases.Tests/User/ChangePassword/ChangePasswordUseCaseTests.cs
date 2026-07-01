using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using MyRecipeBook.Application.UseCases.User.ChangePassword;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.User.ChangePassword;

public class ChangePasswordUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        (var user, var password) = UserBuilder.Build();

        var request = RequestChangePasswordJsonBuilder.Build();
        request.CurrentPassword = password;

        var useCase = CreateUseCase(user, password);

        await useCase.Execute(request).ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenNewPasswordIsEmpty()
    {
        (var user, var password) = UserBuilder.Build();

        var request = new RequestChangePasswordJson
        {
            CurrentPassword = password,
            NewPassword = string.Empty
        };

        var useCase = CreateUseCase(user, password);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.ShouldSatisfyAllConditions(exception =>
        {
            exception.GetStatusCode().ShouldBe(System.Net.HttpStatusCode.BadRequest);
            exception.GetErrorMessages().ShouldSatisfyAllConditions(messages =>
            {
                messages.Count.ShouldBe(1);
                messages.ShouldContain(ResourceMessagesException.VALIDATION_PASSWORD_REQUIRED);
            });
        });
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenCurrentPasswordDoesNotMatch()
    {
        (var user, var _) = UserBuilder.Build();

        var request = RequestChangePasswordJsonBuilder.Build();

        var useCase = CreateUseCase(user, "invalidPassword");

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.ShouldSatisfyAllConditions(exception =>
        {
            exception.GetStatusCode().ShouldBe(System.Net.HttpStatusCode.BadRequest);
            exception.GetErrorMessages().ShouldSatisfyAllConditions(messages =>
            {
                messages.Count.ShouldBe(1);
                messages.ShouldContain(ResourceMessagesException.VALIDATION_CURRENT_PASSWORD);
            });
        });
    }

    private static ChangePasswordUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user, string password)
    {
        var userUpdateRepository = IUserUpdateOnlyRepositoryBuilder.Build();
        var loggedUser = ILoggedUserBuilder.Build(user);
        var passwordHasher = new IPasswordHasherBuilder().VerifyPassword(password).Build();

        return new ChangePasswordUseCase(loggedUser, passwordHasher, userUpdateRepository);
    }
}
