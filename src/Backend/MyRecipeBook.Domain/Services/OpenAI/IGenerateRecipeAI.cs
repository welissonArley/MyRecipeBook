using MyRecipeBook.Domain.Dtos;

namespace MyRecipeBook.Domain.Services.OpenAI;
public interface IGenerateRecipeAI
{
    Task<GeneratedRecipeDto> Generate(IList<string> ingredients);
}
