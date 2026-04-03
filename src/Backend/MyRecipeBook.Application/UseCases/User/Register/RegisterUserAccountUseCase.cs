using FluentValidation.Results;
using Mapster;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.PasswordHashing;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.User.Register;

public class RegisterUserAccountUseCase : IRegisterUserAccountUseCase
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserAccountUseCase(
        IPasswordHasher passwordHasher,
        IUserWriteOnlyRepository userWriteOnlyRepository,
        IUserReadOnlyRepository userReadOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _passwordHasher = passwordHasher;
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _userReadOnlyRepository = userReadOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserAccountJson request)
    {
        await ValidateAndThrowOnFailures(request);

        var user = request.Adapt<Domain.Entities.User>();

        user.Password = _passwordHasher.HashPassword(request.Password);

        await _userWriteOnlyRepository.Add(user);

        await _unitOfWork.Commit();

        return new ResponseRegisteredUserJson
        {
            Name = user.Name
        };
    }

    private async Task ValidateAndThrowOnFailures(RequestRegisterUserAccountJson request)
    {
        var validator = new RegisterUserAccountValidator();

        var result = validator.Validate(request);

        var emailExist = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);
        if (emailExist)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.VALIDATION_EMAIL_ALREADY_EXISTS));
        }

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
