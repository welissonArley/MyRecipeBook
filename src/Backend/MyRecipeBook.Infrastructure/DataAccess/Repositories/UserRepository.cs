using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.User;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

internal sealed class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository
{
    private readonly MyRecipeBookDbContext _dbContext;

    public UserRepository(MyRecipeBookDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(User user) => await _dbContext.Users.AddAsync(user);

    public async Task<bool> ExistActiveUserWithEmail(string email)
    {
        return await _dbContext.Users.AnyAsync(user => user.Active && user.Email.Equals(email));
    }

    public async Task<bool> ExistActiveUserWithId(Guid userId)
    {
        return await _dbContext.Users.AnyAsync(user => user.Active && user.Id == userId);
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(user => user.Active && user.Email.Equals(email));
    }
}