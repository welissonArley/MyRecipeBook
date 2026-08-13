namespace MyRecipeBook.Domain.Repositories.User;

public interface IUserUpdateOnlyRepository
{
    void UpdateProfile(Entities.User user);
    Task UpdatePassword(Guid userId, string passwordHash);
    Task UpdateProfilePictureStatus(Guid userId, bool hasProfilePicture);
}