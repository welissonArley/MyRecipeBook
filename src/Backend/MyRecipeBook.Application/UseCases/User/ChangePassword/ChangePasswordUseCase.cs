using FluentValidation.Results;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.PasswordHashing;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.User.ChangePassword;

public class ChangePasswordUseCase : IChangePasswordUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;

    public ChangePasswordUseCase(
        ILoggedUser loggedUser,
        IPasswordHasher passwordHasher,
        IUserUpdateOnlyRepository userUpdateOnlyRepository)
    {
        _loggedUser = loggedUser;
        _passwordHasher = passwordHasher;
        _userUpdateOnlyRepository = userUpdateOnlyRepository;
    }

    public async Task Execute(RequestChangePasswordJson request)
    {
        var loggedUser = await _loggedUser.Get();

        ValidateAndThrowOnFailures(request, loggedUser);

        var hashedPassword = _passwordHasher.HashPassword(request.NewPassword);

        await _userUpdateOnlyRepository.UpdatePassword(loggedUser.Id, hashedPassword);
    }

    private void ValidateAndThrowOnFailures(RequestChangePasswordJson request, Domain.Entities.User loggedUser)
    {
        var result = new ChangePasswordValidator().Validate(request);

        if (_passwordHasher.VerifyPassword(request.CurrentPassword, loggedUser.Password) == false)
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.VALIDATION_CURRENT_PASSWORD));

        if (result.IsValid == false)
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}