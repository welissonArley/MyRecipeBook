using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.RefreshToken;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

internal sealed class RefreshTokenRepository : IRefreshTokenWriteOnlyRepository, IRefreshTokenReadOnlyRepository
{
    private readonly MyRecipeBookDbContext _dbContext;

    public RefreshTokenRepository(MyRecipeBookDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Replace(RefreshToken refreshToken)
    {
        var existingTokens = await _dbContext.RefreshTokens
            .Where(token => token.UserId == refreshToken.UserId)
            .ToListAsync();

        _dbContext.RefreshTokens.RemoveRange(existingTokens);

        await _dbContext.RefreshTokens.AddAsync(refreshToken);
    }

    public async Task<RefreshToken?> Get(string refreshToken)
    {
        return await _dbContext.RefreshTokens
            .AsNoTracking()
            .SingleOrDefaultAsync(token => token.Active && token.Value.Equals(refreshToken));
    }
}
