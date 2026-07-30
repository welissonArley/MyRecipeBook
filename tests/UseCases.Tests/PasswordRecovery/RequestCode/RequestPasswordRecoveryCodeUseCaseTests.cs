using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.PasswordRecovery.RequestCode;
using MyRecipeBook.Communication.Requests;
using Shouldly;

namespace UseCases.Tests.PasswordRecovery.RequestCode;

public class RequestPasswordRecoveryCodeUseCaseTests
{
    [Fact]
    public async Task Success_WhenUserExists()
    {
        var (user, _) = UserBuilder.Build();

        var request = new RequestPasswordRecoveryJson
        {
            Email = user.Email
        };

        var useCase = CreateUseCase(user);

        await useCase.Execute(request).ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Success_WhenUserDoesNotExist()
    {
        var request = RequestPasswordRecoveryJsonBuilder.Build();

        var useCase = CreateUseCase();

        await useCase.Execute(request).ShouldNotThrowAsync();
    }

    private static RequestPasswordRecoveryCodeUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User? user = null)
    {
        var unitOfWork = IUnitOfWorkBuilder.Build();
        var verificationCodeWriteOnlyRepository = IVerificationCodeWriteOnlyRepositoryBuilder.Build();
        var userReadOnlyRepositoryBuilder = new IUserReadOnlyRepositoryBuilder();
        if (user is not null)
            userReadOnlyRepositoryBuilder.GetByEmail(user);

        return new RequestPasswordRecoveryCodeUseCase(userReadOnlyRepositoryBuilder.Build(), verificationCodeWriteOnlyRepository, unitOfWork);
    }
}
