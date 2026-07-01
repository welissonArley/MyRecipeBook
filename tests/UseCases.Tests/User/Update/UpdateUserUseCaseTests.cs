using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.User.Update;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.User.Update;

public class UpdateUserUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();

        var useCase = CreateUseCase(user);

        await useCase.Execute(request).ShouldNotThrowAsync();

        user.Name.ShouldBe(request.Name);
        user.Email.ShouldBe(request.Email);
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenNameIsEmpty()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase(user);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.ShouldSatisfyAllConditions(exception =>
        {
            exception.GetStatusCode().ShouldBe(System.Net.HttpStatusCode.BadRequest);
            exception.GetErrorMessages().ShouldSatisfyAllConditions(messages =>
            {
                messages.Count.ShouldBe(1);
                messages.ShouldContain(ResourceMessagesException.VALIDATION_NAME_REQUIRED);
            });
        });

        user.Name.ShouldNotBe(request.Name);
        user.Email.ShouldNotBe(request.Email);
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenEmailAlreadyExists()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();

        var useCase = CreateUseCase(user, request.Email);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.ShouldSatisfyAllConditions(exception =>
        {
            exception.GetStatusCode().ShouldBe(System.Net.HttpStatusCode.BadRequest);
            exception.GetErrorMessages().ShouldSatisfyAllConditions(messages =>
            {
                messages.Count.ShouldBe(1);
                messages.ShouldContain(ResourceMessagesException.VALIDATION_EMAIL_ALREADY_EXISTS);
            });
        });

        user.Name.ShouldNotBe(request.Name);
        user.Email.ShouldNotBe(request.Email);
    }

    private static UpdateUserUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user, string? emailThatAlreadyExists = null)
    {
        var unitOfWork = IUnitOfWorkBuilder.Build();
        var userUpdateRepository = IUserUpdateOnlyRepositoryBuilder.Build();
        var loggedUser = ILoggedUserBuilder.Build(user);

        var userReadOnlyRepositoryBuilder = new IUserReadOnlyRepositoryBuilder();
        if (emailThatAlreadyExists.IsNotEmpty())
            userReadOnlyRepositoryBuilder.ExistActiveUserWithEmail(emailThatAlreadyExists);

        return new UpdateUserUseCase(loggedUser, userReadOnlyRepositoryBuilder.Build(), userUpdateRepository, unitOfWork);
    }
}
