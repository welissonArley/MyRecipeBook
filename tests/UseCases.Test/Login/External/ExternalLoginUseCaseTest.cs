using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.Login.External;
using Xunit;

namespace UseCases.Test.Login.External;
public class ExternalLoginUseCaseTest
{
    [Fact]
    public async Task Success_User_Dont_Exist()
    {
        (var user, var _) = UserBuilder.Build();

        var useCase = CreateUseCase();

        var result = await useCase.Execute(name: user.Name, email: user.Email);

        result.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Success_Exist()
    {
        (var user, var _) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(name: user.Name, email: user.Email);

        result.Should().NotBeNullOrEmpty();
    }

    private static ExternalLoginUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User? user = null)
    {
        var userWriteOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
        var userReadOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();

        if (user is not null)
            userReadOnlyRepositoryBuilder.GetByEmail(user);

        return new ExternalLoginUseCase(userReadOnlyRepositoryBuilder.Build(), userWriteOnlyRepository, unitOfWork, accessTokenGenerator);
    }
}
