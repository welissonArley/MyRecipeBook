using Mapster;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.AI;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.GenerateRecipeAI;

public class GenerateRecipeAIUseCase : IGenerateRecipeAIUseCase
{
    private readonly IGenerateRecipeAI _generateRecipeAI;

    public GenerateRecipeAIUseCase(IGenerateRecipeAI generateRecipeAI)
    {
        _generateRecipeAI = generateRecipeAI;
    }

    public async Task<ResponseGeneratedRecipeJson> Execute(RequestGenerateRecipeJson request)
    {
        var generatedRecipe = await _generateRecipeAI.Generate(request.Prompt);
        if (generatedRecipe is null)
            throw new ErrorOnValidationException([ResourceMessagesException.UNABLE_TO_GENERATE_RECIPE]);

        return generatedRecipe.Adapt<ResponseGeneratedRecipeJson>();
    }
}
