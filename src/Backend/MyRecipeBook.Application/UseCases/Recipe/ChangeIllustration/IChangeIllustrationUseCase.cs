namespace MyRecipeBook.Application.UseCases.Recipe.ChangeIllustration;

public interface IChangeIllustrationUseCase
{
    Task Execute(Guid recipeId, Stream recipeIllustration);
}
