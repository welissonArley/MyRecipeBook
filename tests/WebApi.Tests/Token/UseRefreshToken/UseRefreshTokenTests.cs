using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Tests.InlineData;
using WebApi.Tests.Resources;

namespace WebApi.Tests.Token.UseRefreshToken;

public class UseRefreshTokenTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "/authentication/token/refresh";

    private readonly UserIdentityManager _user1;

    public UseRefreshTokenTests(MyRecipeBookApplicationFactory factory) : base(factory)
    {
        _user1 = factory.User1;
    }

    [Fact]
    public async Task Success()
    {
        var currentRefreshToken = _user1.GetRefreshToken();

        var request = new RequestNewTokenJson { RefreshToken = currentRefreshToken };

        var response = await Post(REQUEST_URI, request);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("accessToken").GetString().ShouldNotBeNullOrEmpty();

        var newRefreshToken = responseData.RootElement.GetProperty("refreshToken").GetString();
        newRefreshToken.ShouldNotBeNullOrEmpty();
        newRefreshToken.ShouldNotBe(currentRefreshToken);

        var oldTokenExists = await DbContext.RefreshTokens.AsNoTracking().AnyAsync(token => token.Value == currentRefreshToken);
        oldTokenExists.ShouldBeFalse();

        var newTokenExists = await DbContext.RefreshTokens.AsNoTracking().AnyAsync(token => token.Value == newRefreshToken);
        newTokenExists.ShouldBeTrue();
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task ShouldThrowException_WhenRefreshTokenDoesNotExist(string culture)
    {
        var request = new RequestNewTokenJson { RefreshToken = Guid.NewGuid().ToString() };

        var response = await Post(REQUEST_URI, request, culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedErrorMessage = ResourceMessagesException.ResourceManager.GetString("EXPIRED_REFRESH_TOKEN", new CultureInfo(culture));

        errors.ShouldSatisfyAllConditions(errorsList =>
        {
            errorsList.Count().ShouldBe(1);
            errorsList.ShouldContain(error => error.GetString().IsNotEmpty() && error.GetString()!.Equals(expectedErrorMessage));
        });
    }
}
