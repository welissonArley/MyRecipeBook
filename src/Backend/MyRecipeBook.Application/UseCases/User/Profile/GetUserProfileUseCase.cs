using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Storage;

namespace MyRecipeBook.Application.UseCases.User.Profile;

public class GetUserProfileUseCase : IGetUserProfileUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IStorageService _storageService;

    public GetUserProfileUseCase(ILoggedUser loggedUser, IStorageService storageService)
    {
        _loggedUser = loggedUser;
        _storageService = storageService;
    }

    public async Task<ResponseUserProfileJson> Execute()
    {
        var loggedUser = await _loggedUser.Get();

        return new ResponseUserProfileJson
        {
            Name = loggedUser.Name,
            Email = loggedUser.Email,
            ImageUrl = loggedUser.HasImage ? _storageService.GetProfilePictureUrl(loggedUser) : string.Empty
        };
    }
}
