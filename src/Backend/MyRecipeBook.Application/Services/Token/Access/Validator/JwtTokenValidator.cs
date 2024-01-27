using Microsoft.IdentityModel.Tokens;
using MyRecipeBook.Domain.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyRecipeBook.Application.Services.Token.Access.Validator;

public class JwtTokenValidator(string signingKey) : JwtTokenHandler, IAccessTokenValidator
{
    private readonly string _signingKey = signingKey;

    public Guid ValidateAndGetUserIdentifier(string token)
    {
        var tokenContent = ValidateToken(token);

        var identifier = tokenContent.Claims.First(t => t.Type == ClaimTypes.Sid).Value;

        return Guid.Parse(identifier);
    }

    private ClaimsPrincipal ValidateToken(string token)
    {
        var validationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            IssuerSigningKey = SecurityKey(_signingKey),
            ClockSkew = new TimeSpan(0)
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        return tokenHandler.ValidateToken(token, validationParameters, out _);
    }
}
