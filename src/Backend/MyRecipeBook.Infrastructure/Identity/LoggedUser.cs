using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Infrastructure.DataAccess;

namespace MyRecipeBook.Infrastructure.Identity;

internal sealed class LoggedUser : ILoggedUser
{
    private readonly IAccessTokenProvider _accessTokenProvider;
    private readonly MyRecipeBookDbContext _dbContext;

    public LoggedUser(IAccessTokenProvider accessTokenProvider, MyRecipeBookDbContext dbContext)
    {
        _accessTokenProvider = accessTokenProvider;
        _dbContext = dbContext;
    }

    public async Task<User> Get()
    {
        var userId = GetUserId();

        return await _dbContext
            .Users
            .AsNoTracking()
            .FirstAsync(user => user.Active && user.Id == userId);
    }

    public Guid GetUserId()
    {
        var accessToken = _accessTokenProvider.GetToken();

        var handler = new JsonWebTokenHandler();

        var jsonWebToken = handler.ReadJsonWebToken(accessToken);

        var subject = jsonWebToken.Subject;

        return Guid.Parse(subject);
    }
}