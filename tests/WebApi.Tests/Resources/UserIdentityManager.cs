namespace WebApi.Tests.Resources;

public class UserIdentityManager
{
    private readonly MyRecipeBook.Domain.Entities.User _user;
    private readonly MyRecipeBook.Domain.Entities.Recipe _recipe;
    private readonly string _password;
    private readonly string _accessToken;
    private readonly string _refreshToken;

    public UserIdentityManager(
        MyRecipeBook.Domain.Entities.User user,
        MyRecipeBook.Domain.Entities.Recipe recipe,
        string password,
        string accessToken,
        string refreshToken)
    {
        _user = user;
        _recipe = recipe;
        _password = password;
        _accessToken = accessToken;
        _refreshToken = refreshToken;
    }

    public Guid GetId() => _user.Id;
    public string GetName() => _user.Name;
    public string GetEmail() => _user.Email;
    public string GetPassword() => _password;
    public string GetAccessToken() => _accessToken;
    public string GetRefreshToken() => _refreshToken;
    public MyRecipeBook.Domain.Entities.Recipe GetRecipe() => _recipe;
}
