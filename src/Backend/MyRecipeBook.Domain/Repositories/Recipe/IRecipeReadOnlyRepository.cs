using MyRecipeBook.Domain.Dtos;

namespace MyRecipeBook.Domain.Repositories.Recipe;

public interface IRecipeReadOnlyRepository
{
    Task<Entities.Recipe?> GetById(Guid recipeId, Guid userId);
    Task<IList<RecipeSummaryDto>> GetRecentRecipes(Guid userId);
    Task<IList<RecipeSummaryDto>> FilterRecipes(Guid userId, RecipeFilterDto filter);
}
