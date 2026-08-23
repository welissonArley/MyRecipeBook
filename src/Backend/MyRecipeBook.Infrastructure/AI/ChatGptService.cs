using MyRecipeBook.Domain.AI;
using MyRecipeBook.Domain.Dtos;

namespace MyRecipeBook.Infrastructure.AI;

internal sealed class ChatGptService : IGenerateRecipeAI
{
    public async Task<GeneratedRecipeDto?> Generate(string prompt)
    {
        throw new NotImplementedException();
    }
}
