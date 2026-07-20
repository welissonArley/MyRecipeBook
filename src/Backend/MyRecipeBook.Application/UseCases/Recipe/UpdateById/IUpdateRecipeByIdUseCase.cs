using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCases.Recipe.UpdateById;

public interface IUpdateRecipeByIdUseCase
{
    Task Execute(Guid recipeId, RequestRecipeJson request);
}
