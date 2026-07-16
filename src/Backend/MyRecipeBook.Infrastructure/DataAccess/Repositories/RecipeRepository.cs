using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.Recipe;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

internal sealed class RecipeRepository : IRecipeWriteOnlyRepository, IRecipeReadOnlyRepository
{
    private readonly MyRecipeBookDbContext _dbContext;

    public RecipeRepository(MyRecipeBookDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Recipe recipe)
    {
        await _dbContext.Recipes.AddAsync(recipe);
    }

    public async Task<Recipe?> GetById(Guid recipeId, Guid userId)
    {
        return await _dbContext
            .Recipes
            .AsNoTracking()
            .Include(recipe => recipe.Ingredients)
            .Include(recipe => recipe.DishTypes)
            .Include(recipe => recipe.Instructions.OrderBy(instruction => instruction.Order))
            .FirstOrDefaultAsync(recipe => recipe.Active && recipe.Id == recipeId && recipe.UserId == userId);
    }
}
