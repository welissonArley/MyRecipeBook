namespace MyRecipeBook.Application.UseCases.Recipe.DeleteById;

public interface IDeleteRecipeByIdUseCase
{
    Task Execute(Guid recipeId);
}
