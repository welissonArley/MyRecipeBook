using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Domain.Repositories.VerificationCode;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

internal sealed class VerificationCodeRepository : IVerificationCodeWriteOnlyRepository, IVerificationCodeReadOnlyRepository
{
    private readonly MyRecipeBookDbContext _dbContext;

    public VerificationCodeRepository(MyRecipeBookDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Replace(VerificationCode verificationCode)
    {
        var existingCodes = await _dbContext.VerificationCodes
            .Where(code => code.UserId == verificationCode.UserId && code.Type == verificationCode.Type)
            .ToListAsync();

        _dbContext.VerificationCodes.RemoveRange(existingCodes);

        await _dbContext.VerificationCodes.AddAsync(verificationCode);
    }

    public async Task Delete(VerificationCode verificationCode)
    {
        await _dbContext.VerificationCodes
            .Where(code => code.Id == verificationCode.Id)
            .ExecuteDeleteAsync();
    }

    public async Task<VerificationCode?> Get(Guid userId, string code, VerificationCodeType type)
    {
        return await _dbContext.VerificationCodes
            .AsNoTracking()
            .SingleOrDefaultAsync(verificationCode =>
                verificationCode.UserId == userId &&
                verificationCode.Code == code &&
                verificationCode.Type == type);
    }
}
