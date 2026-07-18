namespace MyRecipeBook.Domain.Repositories.Recipe;

public interface IRecipeWriteOnlyRepository
{
    Task Add(Entities.Recipe recipe);
    Task<bool> DeleteById(Guid recipeId, Guid userId);
}
