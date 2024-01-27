using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Tokens;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Login.DoLogin;

public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _repository;
    private readonly PasswordEncripter _passwordEncripter;
    private readonly IAccesTokenGenerator _accesTokenGenerator;

    public DoLoginUseCase(IUserReadOnlyRepository repository, IAccesTokenGenerator accesTokenGenerator, PasswordEncripter passwordEncripter)
    {
        _repository = repository;
        _passwordEncripter = passwordEncripter;
        _accesTokenGenerator = accesTokenGenerator;
    }

    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
    {
        var encriptedPassword = _passwordEncripter.Encrypt(request.Password);

        var user = await _repository.GetByEmailAndPassword(request.Email, encriptedPassword) ?? throw new InvalidLoginException();
        
        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
            Token = new ResponseTokensJson
            {
                AccessToken = _accesTokenGenerator.Generate(user.UserIdentifier)
            }
        };
    }
}
