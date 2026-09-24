using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Security;
using MyRecipeBook.Application.UseCases.Token.UseRefreshToken;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionsBase;
using Shouldly;
using System.Net;

namespace UseCases.Tests.Token.UseRefreshToken;

public class UseRefreshTokenUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();

        var refreshToken = RefreshTokenBuilder.Build(user);

        var request = new RequestNewTokenJson { RefreshToken = refreshToken.Value };

        var useCase = CreateUseCase(refreshToken, user);

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.AccessToken.ShouldNotBeNullOrEmpty();
        result.RefreshToken.ShouldNotBeNullOrEmpty();
        result.RefreshToken.ShouldNotBe(refreshToken.Value);
    }

    [Fact]
    public async Task ShouldThrowException_WhenRefreshTokenDoesNotExist()
    {
        var request = new RequestNewTokenJson { RefreshToken = "non-existent-token" };

        var useCase = CreateUseCase();

        var exception = await useCase.Execute(request).ShouldThrowAsync<RefreshTokenExpiredException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.Unauthorized);

        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.EXPIRED_REFRESH_TOKEN);
        });
    }

    [Fact]
    public async Task ShouldThrowException_WhenRefreshTokenIsExpired()
    {
        var (user, _) = UserBuilder.Build();

        var refreshToken = RefreshTokenBuilder.Build(user);
        refreshToken.CreatedAt = DateTime.UtcNow.AddDays(-8);

        var request = new RequestNewTokenJson { RefreshToken = refreshToken.Value };

        var useCase = CreateUseCase(refreshToken, user);

        var exception = await useCase.Execute(request).ShouldThrowAsync<RefreshTokenExpiredException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.Unauthorized);

        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.EXPIRED_REFRESH_TOKEN);
        });
    }

    private static UseRefreshTokenUseCase CreateUseCase(
        RefreshToken? refreshToken = null,
        MyRecipeBook.Domain.Entities.User? user = null)
    {
        var refreshTokenReadOnlyRepositoryBuilder = new IRefreshTokenReadOnlyRepositoryBuilder();
        if (refreshToken is not null)
            refreshTokenReadOnlyRepositoryBuilder.Get(refreshToken);

        var userReadOnlyRepositoryBuilder = new IUserReadOnlyRepositoryBuilder();
        if (user is not null)
            userReadOnlyRepositoryBuilder.GetById(user);

        var refreshTokenWriteOnlyRepository = IRefreshTokenWriteOnlyRepositoryBuilder.Build();
        var accessTokenGenerator = IAccessTokenGeneratorBuilder.Build();
        var refreshTokenGenerator = IRefreshTokenGeneratorBuilder.Build();
        var unitOfWork = IUnitOfWorkBuilder.Build();

        return new UseRefreshTokenUseCase(
            refreshTokenReadOnlyRepositoryBuilder.Build(),
            refreshTokenWriteOnlyRepository,
            userReadOnlyRepositoryBuilder.Build(),
            accessTokenGenerator,
            refreshTokenGenerator,
            unitOfWork);
    }
}
