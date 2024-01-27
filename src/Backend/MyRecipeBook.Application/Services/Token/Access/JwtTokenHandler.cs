using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MyRecipeBook.Application.Services.Token.Access;

public class JwtTokenHandler
{
    protected SymmetricSecurityKey SecurityKey(string signingKey)
    {
        var symmetricKey = Encoding.UTF8.GetBytes(signingKey);

        return new SymmetricSecurityKey(symmetricKey);
    }
}
