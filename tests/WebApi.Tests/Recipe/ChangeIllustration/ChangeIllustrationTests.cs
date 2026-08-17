using CommonTestUtilities.Files;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Tests.InlineData;
using WebApi.Tests.Resources;

namespace WebApi.Tests.Recipe.ChangeIllustration;

public class ChangeIllustrationTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "recipes";
    private const string FILE_FIELD_NAME = "recipeIllustration";

    private readonly UserIdentityManager _user1;

    public ChangeIllustrationTests(MyRecipeBookApplicationFactory factory) : base(factory)
    {
        _user1 = factory.User1;
    }

    [Fact]
    public async Task Success()
    {
        var recipe = _user1.GetRecipe();

        var response = await PutFormData(
            $"{REQUEST_URI}/{recipe.Id}/illustration",
            FileBuilder.GetPng(),
            _user1.GetAccessToken(),
            FILE_FIELD_NAME);

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var existImageInStorage = await BlobServiceClient.GetBlobContainerClient(_user1.GetId().ToString())
            .GetBlobClient(recipe.Id.ToString())
            .ExistsAsync();

        existImageInStorage.Value.ShouldBeTrue();
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Error_WhenImageIsTxt(string culture)
    {
        var recipe = _user1.GetRecipe();

        var response = await PutFormData(
            $"{REQUEST_URI}/{recipe.Id}/illustration",
            FileBuilder.GetTxt(),
            _user1.GetAccessToken(),
            FILE_FIELD_NAME,
            culture);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("VALIDATION_ONLY_IMAGES_ACCEPTED", new CultureInfo(culture));

        errors.ShouldSatisfyAllConditions(errorsList =>
        {
            errorsList.Count().ShouldBe(1);
            errorsList.ShouldContain(error => error.GetString().IsNotEmpty() && error.GetString()!.Equals(expectedMessage));
        });
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Error_WhenRecipeNotFound(string culture)
    {
        var response = await PutFormData(
            $"{REQUEST_URI}/{Guid.CreateVersion7()}/illustration",
            FileBuilder.GetPng(),
            _user1.GetAccessToken(),
            FILE_FIELD_NAME,
            culture);

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("VALIDATION_RECIPE_NOT_FOUND", new CultureInfo(culture));

        errors.ShouldSatisfyAllConditions(errorsList =>
        {
            errorsList.Count().ShouldBe(1);
            errorsList.ShouldContain(error => error.GetString().IsNotEmpty() && error.GetString()!.Equals(expectedMessage));
        });
    }
}
