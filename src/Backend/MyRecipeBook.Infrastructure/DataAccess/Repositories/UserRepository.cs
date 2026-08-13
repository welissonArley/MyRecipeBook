using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.User;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

internal sealed class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository, IUserUpdateOnlyRepository
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

    public async Task UpdatePassword(Guid userId, string passwordHash)
    {
        await _dbContext
            .Users
            .Where(user => user.Id == userId)
            .ExecuteUpdateAsync(setter => setter.SetProperty(user => user.Password, passwordHash));
    }

    public void UpdateProfile(User user)
    {
        _dbContext.Users.Attach(user);

        _dbContext.Entry(user).Property(user => user.Name).IsModified = true;
        _dbContext.Entry(user).Property(user => user.Email).IsModified = true;
    }

    public async Task UpdateProfilePictureStatus(Guid userId, bool hasProfilePicture)
    {
        await _dbContext
            .Users
            .Where(user => user.Id == userId)
            .ExecuteUpdateAsync(setter => setter.SetProperty(user => user.HasImage, hasProfilePicture));
    }
}