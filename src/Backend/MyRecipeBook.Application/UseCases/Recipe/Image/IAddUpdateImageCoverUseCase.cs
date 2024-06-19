using Microsoft.AspNetCore.Http;

namespace MyRecipeBook.Application.UseCases.Recipe.Image;
public interface IAddUpdateImageCoverUseCase
{
    Task Execute(long recipeId, IFormFile file);
}
