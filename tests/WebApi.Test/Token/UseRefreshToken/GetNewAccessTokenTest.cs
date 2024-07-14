using FluentAssertions;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;
using Xunit;

namespace WebApi.Test.Token.UseRefreshToken;
public class GetNewAccessTokenTest : MyRecipeBookClassFixture
{
    private const string METHOD = "token";

    private readonly string _userRefreshToken;

    public GetNewAccessTokenTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userRefreshToken = factory.GetRefreshToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestNewTokenJson
        {
            RefreshToken = _userRefreshToken
        };

        var response = await DoPost($"{METHOD}/refresh-token", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("accessToken").GetString().Should().NotBeNullOrWhiteSpace();
        responseData.RootElement.GetProperty("refreshToken").GetString().Should().NotBeNullOrWhiteSpace();
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Login_Invalid(string culture)
    {
        var request = new RequestNewTokenJson
        {
            RefreshToken = "InvalidRefreshToken"
        };

        var response = await DoPost($"{METHOD}/refresh-token", request, culture: culture);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("EXPIRED_SESSION", new CultureInfo(culture));

        errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
