using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Repositories.VerificationCode;
using System.Buffers.Text;
using System.Security.Cryptography;

namespace MyRecipeBook.Application.UseCases.Login.WithExternalProvider;

public class LoginWithExternalProviderUseCase : ILoginWithExternalProviderUseCase
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
    private readonly IVerificationCodeWriteOnlyRepository _verificationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LoginWithExternalProviderUseCase(
        IUserReadOnlyRepository userReadOnlyRepository,
        IUserWriteOnlyRepository userWriteOnlyRepository,
        IVerificationCodeWriteOnlyRepository verificationRepository,
        IUnitOfWork unitOfWork)
    {
        _userReadOnlyRepository = userReadOnlyRepository;
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _verificationRepository = verificationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Execute(ExternalLoginInfo externalLoginInfo)
    {
        var user = await _userReadOnlyRepository.GetByEmail(externalLoginInfo.Email);
        if (user is null)
        {
            user = new Domain.Entities.User
            {
                Email = externalLoginInfo.Email,
                Name = externalLoginInfo.Name,
                Password = "-"
            };

            await _userWriteOnlyRepository.Add(user);
        }

        var verificationCode = new Domain.Entities.VerificationCode
        {
            UserId = user.Id,
            Code = GenerateCode(),
            Type = VerificationCodeType.ExternalLoginExchange
        };

        await _verificationRepository.Replace(verificationCode);

        await _unitOfWork.Commit();

        return verificationCode.Code;
    }

    private static string GenerateCode()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);

        return Base64Url.EncodeToString(bytes);
    }
}
