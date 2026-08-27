using CommonTestUtilities.Requests;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Tests.InlineData;
using WebApi.Tests.Resources;

namespace WebApi.Tests.Recipe.GenerateRecipeAI;

public class GenerateRecipeTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "recipes/generate";

    private readonly UserIdentityManager _user1;

    public GenerateRecipeTests(MyRecipeBookApplicationFactory factory) : base(factory)
    {
        _user1 = factory.User1;
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestGenerateRecipeJsonBuilder.Build();

        var response = await Post(REQUEST_URI, request, _user1.GetAccessToken());

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("title").GetString().ShouldNotBeNullOrEmpty();
        responseData.RootElement.GetProperty("ingredients").EnumerateArray().Count().ShouldBeGreaterThan(0);
        responseData.RootElement.GetProperty("instructions").EnumerateArray().Count().ShouldBeGreaterThan(0);
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Error_WhenPromptIsNotARecipe(string culture)
    {
        var request = RequestGenerateRecipeJsonBuilder.Build();
        request.Prompt = string.Empty;

        var response = await Post(REQUEST_URI, request, _user1.GetAccessToken(), culture);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("UNABLE_TO_GENERATE_RECIPE", new CultureInfo(culture));

        errors.ShouldSatisfyAllConditions(errorsList =>
        {
            errorsList.Count().ShouldBe(1);
            errorsList.ShouldContain(error => error.GetString().IsNotEmpty() && error.GetString()!.Equals(expectedMessage));
        });
    }
}
