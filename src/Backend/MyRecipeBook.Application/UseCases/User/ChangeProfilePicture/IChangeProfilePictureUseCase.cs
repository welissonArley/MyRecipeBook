namespace MyRecipeBook.Application.UseCases.User.ChangeProfilePicture;

public interface IChangeProfilePictureUseCase
{
    Task Execute(Stream profilePicture);
}
