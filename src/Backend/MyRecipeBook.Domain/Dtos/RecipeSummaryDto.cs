namespace MyRecipeBook.Domain.Dtos;

public record RecipeSummaryDto(Guid Id, string Title, bool HasImage);