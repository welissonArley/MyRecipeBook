using CommonTestUtilities.Requests;
using Shouldly;
using System.Net;

namespace WebApi.Tests.Recipe.GenerateRecipeAI;

public class GenerateRecipeInvalidTokenTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "recipes/generate";

    private readonly string _tokenUserNotExistDatabase;

    public GenerateRecipeInvalidTokenTests(MyRecipeBookApplicationFactory factory) : base(factory)
    {
        _tokenUserNotExistDatabase = factory.TOKEN_USER_NOT_FOUND_IN_DATABASE;
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenAccessTokenIsInvalid()
    {
        var request = RequestGenerateRecipeJsonBuilder.Build();

        var response = await Post(REQUEST_URI, request, accessToken: "tokenInvalid");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenAccessTokenIsMissing()
    {
        var request = RequestGenerateRecipeJsonBuilder.Build();

        var response = await Post(REQUEST_URI, request, accessToken: string.Empty);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenUserFromAccessTokenDoesNotExist()
    {
        var request = RequestGenerateRecipeJsonBuilder.Build();

        var response = await Post(REQUEST_URI, request, accessToken: _tokenUserNotExistDatabase);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
