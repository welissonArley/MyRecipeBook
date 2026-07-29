namespace MyRecipeBook.Domain.Repositories.VerificationCode;

public interface IVerificationCodeWriteOnlyRepository
{
    Task Add(Entities.VerificationCode verificationCode);
}
