using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Domain.Extensions;

namespace MyRecipeBook.API.Controllers;

[Route("[controller]")]
[ApiController]
public class MyRecipeBookBaseController : ControllerBase
{
    protected static bool IsNotAuthenticated(AuthenticateResult authenticate)
    {
        return authenticate.Succeeded.IsFalse()
            || authenticate.Principal is null
            || authenticate.Principal.Identities.Any(id => id.IsAuthenticated).IsFalse();
    }
}
