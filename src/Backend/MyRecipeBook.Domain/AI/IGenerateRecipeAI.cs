using MyRecipeBook.Domain.Dtos;

namespace MyRecipeBook.Domain.AI;

public interface IGenerateRecipeAI
{
    Task<GeneratedRecipeDto?> Generate(string prompt);
}
