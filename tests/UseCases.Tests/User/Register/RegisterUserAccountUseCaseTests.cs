using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using MyRecipeBook.Application.UseCases.User.Register;
using Shouldly;

namespace UseCases.Tests.User.Register;

public class RegisterUserAccountUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserAccountJsonBuilder.Build();

        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Tokens.ShouldNotBeNull();
        result.Name.ShouldBe(request.Name);
        result.Tokens.AccessToken.ShouldBeNullOrEmpty();
        result.Tokens.RefreshToken.ShouldBeNullOrEmpty();
    }

    private RegisterUserAccountUseCase CreateUseCase()
    {
        var unitOfWork = IUnitOfWorkBuilder.Build();
        var userWriteOnlyRepository = IUserWriteOnlyRepositoryBuilder.Build();
        var userReadOnlyRepository = new IUserReadOnlyRepositoryBuilder().Build();
        var passwordHasher = new IPasswordHasherBuilder().Build();

        return new RegisterUserAccountUseCase(passwordHasher, userWriteOnlyRepository, userReadOnlyRepository, unitOfWork);
    }
}
