namespace MyRecipeBook.Domain.Repositories.Recipe;

public interface IRecipeUpdateOnlyRepository
{
    Task<Entities.Recipe?> GetById(Guid recipeId, Guid userId);
}
