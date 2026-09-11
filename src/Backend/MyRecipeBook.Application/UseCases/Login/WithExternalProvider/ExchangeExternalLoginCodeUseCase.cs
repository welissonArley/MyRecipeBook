using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Repositories.VerificationCode;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Domain.Storage;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Login.WithExternalProvider;

public class ExchangeExternalLoginCodeUseCase : IExchangeExternalLoginCodeUseCase
{
    private const int ExpirationTimeInSeconds = 30;

    private readonly IVerificationCodeReadOnlyRepository _verificationCodeReadOnlyRepository;
    private readonly IVerificationCodeWriteOnlyRepository _verificationCodeWriteOnlyRepository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IStorageService _storageService;

    public ExchangeExternalLoginCodeUseCase(
        IVerificationCodeReadOnlyRepository verificationCodeReadOnlyRepository,
        IUserReadOnlyRepository userReadOnlyRepository,
        IVerificationCodeWriteOnlyRepository verificationCodeWriteOnlyRepository,
        IAccessTokenGenerator accessTokenGenerator,
        IStorageService storageService)
    {
        _verificationCodeReadOnlyRepository = verificationCodeReadOnlyRepository;
        _userReadOnlyRepository = userReadOnlyRepository;
        _verificationCodeWriteOnlyRepository = verificationCodeWriteOnlyRepository;
        _accessTokenGenerator = accessTokenGenerator;
        _storageService = storageService;
    }

    public async Task<ResponseRegisteredUserJson> Execute(RequestExternalLoginJson request)
    {
        var verificationCode = await _verificationCodeReadOnlyRepository.GetExternalLoginCode(request.Code);
        if (verificationCode is null)
            throw new ErrorOnValidationException([ResourceMessagesException.VERIFICATION_CODE_INVALID]);

        var isCodeValid = verificationCode.CreatedAt.AddSeconds(ExpirationTimeInSeconds) >= DateTime.UtcNow;
        if (isCodeValid == false)
            throw new ErrorOnValidationException([ResourceMessagesException.VERIFICATION_CODE_INVALID]);

        await _verificationCodeWriteOnlyRepository.Delete(verificationCode);

        var user = await _userReadOnlyRepository.GetById(verificationCode.UserId);

        return new ResponseRegisteredUserJson
        {
            Name = user!.Name,
            ImageUrl = user.HasImage ? _storageService.GetProfilePictureUrl(user) : string.Empty,
            Tokens = new ResponseTokensJson
            {
                AccessToken = _accessTokenGenerator.Generate(user)
            }
        };
    }
}
