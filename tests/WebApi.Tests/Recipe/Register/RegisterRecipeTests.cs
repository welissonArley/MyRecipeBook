using CommonTestUtilities.Requests;
using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Tests.InlineData;
using WebApi.Tests.Resources;

namespace WebApi.Tests.Recipe.Register;

public class RegisterRecipeTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "/recipes";

    private readonly UserIdentityManager _user1;

    public RegisterRecipeTests(MyRecipeBookApplicationFactory factory) : base(factory)
    {
        _user1 = factory.User1;
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestRecipeJsonBuilder.Build();

        var response = await Post(REQUEST_URI, request, accessToken: _user1.GetAccessToken());

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("title").GetString().ShouldBe(request.Title);

        var recipeId = responseData.RootElement.GetProperty("id").GetGuid();

        var recipeExists = await DbContext.Recipes.AnyAsync(recipe =>
            recipe.Id == recipeId &&
            recipe.Active &&
            recipe.Title.Equals(request.Title) &&
            recipe.UserId == _user1.GetId());

        recipeExists.ShouldBeTrue();
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Validate_ShouldBeAnErrorResponse_WhenTitleIsEmpty(string culture)
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Title = string.Empty;

        var response = await Post(REQUEST_URI, request, accessToken: _user1.GetAccessToken(), culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedErrorMessage = ResourceMessagesException.ResourceManager.GetString("VALIDATION_TITLE_REQUIRED", new CultureInfo(culture));

        errors.ShouldSatisfyAllConditions(errorsList =>
        {
            errorsList.Count().ShouldBe(1);
            errorsList.ShouldContain(error => error.GetString().IsNotEmpty() && error.GetString()!.Equals(expectedErrorMessage));
        });

        var recipeExists = await DbContext.Recipes.AnyAsync(recipe =>
            recipe.Active &&
            recipe.UserId == _user1.GetId() &&
            recipe.Title.Equals(request.Title));

        recipeExists.ShouldBeFalse();
    }
}
