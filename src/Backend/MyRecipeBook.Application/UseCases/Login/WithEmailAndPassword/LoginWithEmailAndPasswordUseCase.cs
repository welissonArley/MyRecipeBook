using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.PasswordHashing;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Exception.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Login.WithEmailAndPassword;

public class LoginWithEmailAndPasswordUseCase : ILoginWithEmailAndPasswordUseCase
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public LoginWithEmailAndPasswordUseCase(
        IPasswordHasher passwordHasher,
        IUserReadOnlyRepository userReadOnlyRepository,
        IAccessTokenGenerator accessTokenGenerator)
    {
        _passwordHasher = passwordHasher;
        _userReadOnlyRepository = userReadOnlyRepository;
        _accessTokenGenerator = accessTokenGenerator;
    }

    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
    {
        var user = await _userReadOnlyRepository.GetByEmail(request.Email);
        if (user is null)
            throw new InvalidLoginException();

        var isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user.Password);
        if(isPasswordValid == false)
            throw new InvalidLoginException();

        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
            Tokens = new ResponseTokensJson
            {
                AccessToken = _accessTokenGenerator.Generate(user)
            }
        };
    }
}