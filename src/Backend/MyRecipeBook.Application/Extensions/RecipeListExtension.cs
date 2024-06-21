using AutoMapper;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Services.Storage;

namespace MyRecipeBook.Application.Extensions;
public static class RecipeListExtension
{
    public static async Task<IList<ResponseShortRecipeJson>> MapToShortRecipeJson(
        this IList<Recipe> recipes,
        User user,
        IBlobStorageService blobStorageService,
        IMapper mapper)
    {
        var result = recipes.Select(async recipe =>
        {
            var response = mapper.Map<ResponseShortRecipeJson>(recipe);

            if (recipe.ImageIdentifier.NotEmpty())
            {
                response.ImageUrl = await blobStorageService.GetFileUrl(user, recipe.ImageIdentifier);
            }

            return response;
        });

        var response = await Task.WhenAll(result);

        return response;
    }
}
