using MyRecipeBook.Domain.Enums;

namespace MyRecipeBook.Domain.Entities;

public class VerificationCode : EntityBase
{
    public string Code { get; set; } = string.Empty;
    public VerificationCodeType Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid UserId { get; set; }
}
