using Konscious.Security.Cryptography;
using MyRecipeBook.Domain.Security.PasswordHashing;
using System.Security.Cryptography;
using System.Text;

namespace MyRecipeBook.Infrastructure.Security.PasswordHashing;

internal sealed class Argon2PasswordHasher : IPasswordHasher
{
    private const int DEGREE_OF_PARALLELISM = 1;
    private const int ITERATIONS = 2;
    private const int MEMORY_SIZE = 20 * 1024; // 20 MB
    private const int SALT_SIZE = 16;
    private const int HASH_SIZE = 32;

    public string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SALT_SIZE);

        var passwordBytes = Encoding.UTF8.GetBytes(password);

        var hashAlgorithm = new Argon2id(passwordBytes)
        {
            DegreeOfParallelism = DEGREE_OF_PARALLELISM,
            Iterations = ITERATIONS,
            MemorySize = MEMORY_SIZE,
            Salt = salt
        };

        var hash = hashAlgorithm.GetBytes(HASH_SIZE);

        var combinedBytes = new byte[hash.Length + salt.Length];

        salt.CopyTo(combinedBytes);
        hash.CopyTo(combinedBytes, index: salt.Length);

        return Convert.ToBase64String(combinedBytes);
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        throw new NotImplementedException();
    }
}
