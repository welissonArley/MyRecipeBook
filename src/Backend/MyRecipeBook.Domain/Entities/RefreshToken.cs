namespace MyRecipeBook.Domain.Entities;
public class RefreshToken : EntityBase
{
    public required string Value { get; set; } = string.Empty;
    public required long UserId { get; set; }
    public User User { get; set; } = default!;
}