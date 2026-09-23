using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.RefreshToken;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Exception.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Token.UseRefreshToken;

public class UseRefreshTokenUseCase : IUseRefreshTokenUseCase
{
    private const int ExpirationTimeInDays = 7;

    private readonly IRefreshTokenReadOnlyRepository _refreshTokenReadOnlyRepository;
    private readonly IRefreshTokenWriteOnlyRepository _refreshTokenWriteOnlyRepository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public UseRefreshTokenUseCase(
        IRefreshTokenReadOnlyRepository refreshTokenReadOnlyRepository,
        IRefreshTokenWriteOnlyRepository refreshTokenWriteOnlyRepository,
        IUserReadOnlyRepository userReadOnlyRepository,
        IAccessTokenGenerator accessTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator,
        IUnitOfWork unitOfWork)
    {
        _refreshTokenReadOnlyRepository = refreshTokenReadOnlyRepository;
        _refreshTokenWriteOnlyRepository = refreshTokenWriteOnlyRepository;
        _userReadOnlyRepository = userReadOnlyRepository;
        _accessTokenGenerator = accessTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseTokensJson> Execute(RequestNewTokenJson request)
    {
        var refreshToken = await _refreshTokenReadOnlyRepository.Get(request.RefreshToken);
        if (refreshToken is null)
            throw new RefreshTokenExpiredException();

        var isValid = refreshToken.CreatedAt.AddDays(ExpirationTimeInDays) >= DateTime.UtcNow;
        if (isValid == false)
            throw new RefreshTokenExpiredException();

        var user = await _userReadOnlyRepository.GetById(refreshToken.UserId);

        var newRefreshToken = new RefreshToken
        {
            Value = _refreshTokenGenerator.Generate(),
            UserId = refreshToken.UserId
        };

        await _refreshTokenWriteOnlyRepository.Replace(newRefreshToken);

        await _unitOfWork.Commit();

        return new ResponseTokensJson
        {
            AccessToken = _accessTokenGenerator.Generate(user!),
            RefreshToken = newRefreshToken.Value
        };
    }
}
