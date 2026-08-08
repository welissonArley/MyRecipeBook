using CommonTestUtilities.Files;
using Shouldly;
using System.Net;

namespace WebApi.Tests.User.ChangeProfilePicture;

public class ChangeProfilePictureInvalidTokenTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "users/profile-picture";
    private const string FILE_FIELD_NAME = "profilePicture";

    private readonly string _tokenUserNotExistDatabase;

    public ChangeProfilePictureInvalidTokenTests(MyRecipeBookApplicationFactory factory) : base(factory)
    {
        _tokenUserNotExistDatabase = factory.TOKEN_USER_NOT_FOUND_IN_DATABASE;
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenAccessTokenIsInvalid()
    {
        var response = await PutFormData(REQUEST_URI, FileBuilder.GetJpeg(), "tokenInvalid", FILE_FIELD_NAME);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenAccessTokenIsMissing()
    {
        var response = await PutFormData(REQUEST_URI, FileBuilder.GetJpeg(), string.Empty, FILE_FIELD_NAME);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenUserFromAccessTokenDoesNotExist()
    {
        var response = await PutFormData(REQUEST_URI, FileBuilder.GetJpeg(), _tokenUserNotExistDatabase, FILE_FIELD_NAME);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
