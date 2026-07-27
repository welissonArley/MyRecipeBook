using MyRecipeBook.Domain.Dtos;

namespace MyRecipeBook.Domain.Repositories.Recipe;

public interface IRecipeReadOnlyRepository
{
    Task<Entities.Recipe?> GetById(Guid recipeId, Guid userId);
    Task<IList<Entities.Recipe>> GetRecentRecipes(Guid userId);
    Task<IList<Entities.Recipe>> FilterRecipes(Guid userId, RecipeFilterDto filter);
}
