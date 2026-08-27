using Moq;
using MyRecipeBook.Domain.AI;
using MyRecipeBook.Domain.Extensions;

namespace CommonTestUtilities.AI;

public class IGenerateRecipeAIBuilder
{
    public static IGenerateRecipeAI Build()
    {
        var mock = new Mock<IGenerateRecipeAI>();

        // Prompt vazio simula "não é receita" (sera devolvido null como resposta).
        // Qualquer prompt com conteúdo devolve uma receita
        mock.Setup(service => service.Generate(It.IsAny<string>()))
            .ReturnsAsync((string prompt) =>
            {
                return prompt.IsEmpty() ? null : GeneratedRecipeDtoBuilder.Build();
            });

        return mock.Object;
    }
}
