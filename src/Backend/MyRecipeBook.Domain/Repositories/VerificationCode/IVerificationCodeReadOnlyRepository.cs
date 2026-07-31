namespace MyRecipeBook.Domain.Repositories.VerificationCode;

public interface IVerificationCodeReadOnlyRepository
{
    Task<Entities.VerificationCode?> Get(Guid userId, string code, Enums.VerificationCodeType type);
}
