using CommonTestUtilities.Files;
using Shouldly;
using System.Net;

namespace WebApi.Tests.Recipe.ChangeIllustration;

public class ChangeIllustrationInvalidTokenTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "recipes";
    private const string FILE_FIELD_NAME = "recipeIllustration";

    private readonly string _tokenUserNotExistDatabase;

    public ChangeIllustrationInvalidTokenTests(MyRecipeBookApplicationFactory factory) : base(factory)
    {
        _tokenUserNotExistDatabase = factory.TOKEN_USER_NOT_FOUND_IN_DATABASE;
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenAccessTokenIsInvalid()
    {
        var response = await PutFormData($"{REQUEST_URI}/{Guid.CreateVersion7()}/illustration", FileBuilder.GetJpeg(), "tokenInvalid", FILE_FIELD_NAME);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenAccessTokenIsMissing()
    {
        var response = await PutFormData($"{REQUEST_URI}/{Guid.CreateVersion7()}/illustration", FileBuilder.GetJpeg(), string.Empty, FILE_FIELD_NAME);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenUserFromAccessTokenDoesNotExist()
    {
        var response = await PutFormData($"{REQUEST_URI}/{Guid.CreateVersion7()}/illustration", FileBuilder.GetJpeg(), _tokenUserNotExistDatabase, FILE_FIELD_NAME);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
