using MyRecipeBook.Communication.Requests;
using Shouldly;
using System.Net;

namespace WebApi.Tests.Recipe.Register;

public class RegisterRecipeInvalidTokenTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "recipes";

    private readonly string _tokenUserNotExistDatabase;

    public RegisterRecipeInvalidTokenTests(MyRecipeBookApplicationFactory factory) : base(factory)
    {
        _tokenUserNotExistDatabase = factory.TOKEN_USER_NOT_FOUND_IN_DATABASE;
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenAccessTokenIsInvalid()
    {
        var request = new RequestRecipeJson();

        var response = await Post(REQUEST_URI, request, accessToken: "tokenInvalid");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenAccessTokenIsMissing()
    {
        var request = new RequestRecipeJson();

        var response = await Post(REQUEST_URI, request, accessToken: string.Empty);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenUserFromAccessTokenDoesNotExist()
    {
        var request = new RequestRecipeJson();

        var response = await Post(REQUEST_URI, request, _tokenUserNotExistDatabase);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
