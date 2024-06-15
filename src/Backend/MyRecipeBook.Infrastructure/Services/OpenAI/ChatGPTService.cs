using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Services.OpenAI;
using OpenAI_API;
using OpenAI_API.Chat;

namespace MyRecipeBook.Infrastructure.Services.OpenAI;
public class ChatGPTService : IGenerateRecipeAI
{
    private const string CHAT_MODEL = "gpt-4o";

    private readonly IOpenAIAPI _openAIAPI;

    public ChatGPTService(IOpenAIAPI openAIAPI)
    {
        _openAIAPI = openAIAPI;
    }

    public async Task<GeneratedRecipeDto> Generate(IList<string> ingredients)
    {
        var conversation = _openAIAPI.Chat.CreateConversation(new ChatRequest { Model = CHAT_MODEL });

        conversation.AppendSystemMessage(ResourceOpenAI.STARTING_GENERATE_RECIPE);

        conversation.AppendUserInput(string.Join(";", ingredients));

        var response = await conversation.GetResponseFromChatbotAsync();

        return null;
    }
}
