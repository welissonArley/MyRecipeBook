using CommonTestUtilities.AI;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.Mappings;
using MyRecipeBook.Application.UseCases.Recipe.GenerateRecipeAI;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionsBase;
using Shouldly;
using System.Net;

namespace UseCases.Tests.Recipe.GenerateRecipeAI;

public class GenerateRecipeAIUseCaseTests
{
    static GenerateRecipeAIUseCaseTests()
    {
        MapsterConfiguration.Configure();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestGenerateRecipeJsonBuilder.Build();

        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Title.ShouldNotBeNullOrEmpty();
        result.Description.ShouldNotBeNullOrEmpty();
        result.Difficulty.ShouldNotBeNullOrEmpty();
        result.Servings.ShouldNotBeNullOrEmpty();
        result.Ingredients.ShouldNotBeEmpty();
        result.Instructions.ShouldNotBeEmpty();
        result.Image.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task Error_WhenPromptIsNotARecipe()
    {
        var request = RequestGenerateRecipeJsonBuilder.Build();
        request.Prompt = string.Empty;

        var useCase = CreateUseCase();

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);

        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.UNABLE_TO_GENERATE_RECIPE);
        });
    }

    private static GenerateRecipeAIUseCase CreateUseCase()
    {
        var generateRecipeAI = IGenerateRecipeAIBuilder.Build();

        return new GenerateRecipeAIUseCase(generateRecipeAI);
    }
}
