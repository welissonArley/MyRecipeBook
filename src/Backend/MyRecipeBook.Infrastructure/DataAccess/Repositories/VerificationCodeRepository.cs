using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.VerificationCode;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

internal sealed class VerificationCodeRepository : IVerificationCodeWriteOnlyRepository
{
    private readonly MyRecipeBookDbContext _dbContext;

    public VerificationCodeRepository(MyRecipeBookDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(VerificationCode verificationCode) => await _dbContext.VerificationCodes.AddAsync(verificationCode);
}
