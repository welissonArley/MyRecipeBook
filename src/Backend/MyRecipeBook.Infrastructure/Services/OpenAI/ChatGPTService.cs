using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Services.OpenAI;
using OpenAI_API;
using OpenAI_API.Chat;

namespace MyRecipeBook.Infrastructure.Services.OpenAI;
public class ChatGptService : IGenerateRecipeAI
{
    private const string CHAT_MODEL = "gpt-4o";

    private readonly IOpenAIAPI _openAIAPI;

    public ChatGptService(IOpenAIAPI openAIAPI)
    {
        _openAIAPI = openAIAPI;
    }

    public async Task<GeneratedRecipeDto> Generate(IList<string> ingredients)
    {
        var conversation = _openAIAPI.Chat.CreateConversation(new ChatRequest { Model = CHAT_MODEL });

        conversation.AppendSystemMessage(ResourceOpenAI.STARTING_GENERATE_RECIPE);

        conversation.AppendUserInput(string.Join(";", ingredients));

        var response = await conversation.GetResponseFromChatbotAsync();

        var responseList = response
            .Split("\n")
            .Where(response => response.Trim().Equals(string.Empty).IsFalse())
            .Select(item => item.Replace("[", "").Replace("]", ""))
            .ToList();

        var step = 1;

        return new GeneratedRecipeDto
        {
            Title = responseList[0],
            CookingTime = (CookingTime)Enum.Parse(typeof(CookingTime), responseList[1]),
            Ingredients = responseList[2].Split(";"),
            Instructions = responseList[3].Split("@").Select(instruction => new GeneratedInstructionDto
            {
                Text = instruction.Trim(),
                Step = step++
            }).ToList()
        };
    }
}
