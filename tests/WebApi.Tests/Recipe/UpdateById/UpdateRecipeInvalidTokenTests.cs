using MyRecipeBook.Communication.Requests;
using Shouldly;
using System.Net;

namespace WebApi.Tests.Recipe.UpdateById;

public class UpdateRecipeInvalidTokenTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "recipes";

    private readonly string _tokenUserNotExistDatabase;

    public UpdateRecipeInvalidTokenTests(MyRecipeBookApplicationFactory factory) : base(factory)
    {
        _tokenUserNotExistDatabase = factory.TOKEN_USER_NOT_FOUND_IN_DATABASE;
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenAccessTokenIsInvalid()
    {
        var request = new RequestRecipeJson();

        var response = await Put($"{REQUEST_URI}/{Guid.CreateVersion7()}", request, accessToken: "tokenInvalid");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenAccessTokenIsMissing()
    {
        var request = new RequestRecipeJson();

        var response = await Put($"{REQUEST_URI}/{Guid.CreateVersion7()}", request, accessToken: string.Empty);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenUserFromAccessTokenDoesNotExist()
    {
        var request = new RequestRecipeJson();

        var response = await Put($"{REQUEST_URI}/{Guid.CreateVersion7()}", request, _tokenUserNotExistDatabase);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
