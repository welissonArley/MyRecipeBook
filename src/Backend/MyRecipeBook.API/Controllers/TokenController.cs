using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Application.UseCases.Token.RefreshToken;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.API.Controllers;

public class TokenController : MyRecipeBookBaseController
{
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(ResponseTokensJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshToken(
        [FromServices] IUseRefreshTokenUseCase useCase,
        [FromBody] RequestNewTokenJson request)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }
}
