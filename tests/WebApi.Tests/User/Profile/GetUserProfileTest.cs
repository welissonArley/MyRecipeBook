using Shouldly;
using System.Net;
using System.Text.Json;
using WebApi.Tests.Resources;

namespace WebApi.Tests.User.Profile;

public class GetUserProfileTest : BaseIntegrationTest
{
    private const string REQUEST_URI = "/users";

    private readonly UserIdentityManager _user1;

    public GetUserProfileTest(MyRecipeBookApplicationFactory factory) : base(factory)
    {
        _user1 = factory.User1;
    }

    [Fact]
    public async Task Success()
    {
        var response = await Get(REQUEST_URI, accessToken: _user1.GetAccessToken());

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().ShouldBe(_user1.GetName());
        responseData.RootElement.GetProperty("email").GetString().ShouldBe(_user1.GetEmail());
    }
}
