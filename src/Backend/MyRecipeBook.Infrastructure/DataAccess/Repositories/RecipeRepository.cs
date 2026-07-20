using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.Recipe;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

internal sealed class RecipeRepository : IRecipeWriteOnlyRepository, IRecipeReadOnlyRepository, IRecipeUpdateOnlyRepository
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

    public async Task<bool> DeleteById(Guid recipeId, Guid userId)
    {
        var rows = await _dbContext
            .Recipes
            .Where(recipe => recipe.Active && recipe.Id == recipeId && recipe.UserId == userId)
            .ExecuteDeleteAsync();

        return rows > 0;
    }

    async Task<Recipe?> IRecipeReadOnlyRepository.GetById(Guid recipeId, Guid userId)
    {
        return await GetFullRecipe()
            .AsNoTracking()
            .FirstOrDefaultAsync(recipe => recipe.Active && recipe.Id == recipeId && recipe.UserId == userId);
    }

    async Task<Recipe?> IRecipeUpdateOnlyRepository.GetById(Guid recipeId, Guid userId)
    {
        return await GetFullRecipe()
            .FirstOrDefaultAsync(recipe => recipe.Active && recipe.Id == recipeId && recipe.UserId == userId);
    }

    private IIncludableQueryable<Recipe, IOrderedEnumerable<RecipeInstruction>> GetFullRecipe()
    {
        return _dbContext
            .Recipes
            .Include(recipe => recipe.Ingredients)
            .Include(recipe => recipe.DishTypes)
            .Include(recipe => recipe.Instructions.OrderBy(instruction => instruction.Order));
    }
}
