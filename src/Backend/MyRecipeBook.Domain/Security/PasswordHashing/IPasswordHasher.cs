namespace MyRecipeBook.Domain.Security.PasswordHashing;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string passwordHash);
}
