using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyRecipeBook.Api.Configuration;
using MyRecipeBook.Application.UseCases.Login.WithEmailAndPassword;
using MyRecipeBook.Application.UseCases.PasswordRecovery.RequestCode;
using MyRecipeBook.Application.UseCases.PasswordRecovery.ResetPassword;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Extensions;
using System.Security.Claims;

namespace MyRecipeBook.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromServices] ILoginWithEmailAndPasswordUseCase useCase,
        [FromBody] RequestLoginJson request)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }

    [HttpGet("external-login/google/start")]
    public async Task<IActionResult> StartGoogleLogin(
        [FromQuery] string client,
        [FromServices] IOptions<ExternalLoginReturnUrlOptions> externalLoginReturnUrlOptions)
    {
        var auth = await Request.HttpContext.AuthenticateAsync(GoogleOpenIdConnectDefaults.AuthenticationScheme);
        if (auth.Succeeded == false)
            return Challenge(GoogleOpenIdConnectDefaults.AuthenticationScheme);

        var claims = auth.Principal!.Identities.First().Claims;

        var email = claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email))!.Value;
        var name = claims.FirstOrDefault(c => c.Type.Equals("name"))!.Value;

        var code = "ABC123"; // TODO : usar usecase aqui

        await Request.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        var frontEndBase = client?.ToLowerInvariant() switch
        {
            "site" => externalLoginReturnUrlOptions.Value.Site,
            "app" => externalLoginReturnUrlOptions.Value.App,
            _ => null
        };

        if (frontEndBase.IsEmpty())
            return BadRequest();

        return Redirect($"{frontEndBase}?code={code}");
    }

    [HttpPost("external-login/exchange")]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ExchangeExternalLoginCode([FromBody] RequestExternalLoginJson request)
    {
        return Ok();
    }

    [HttpPost("password-recovery")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> PasswordRecovery(
        [FromServices] IRequestPasswordRecoveryCodeUseCase useCase,
        [FromBody] RequestPasswordRecoveryJson request)
    {
        await useCase.Execute(request);

        return Accepted();
    }

    [HttpPost("password-recovery/reset")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword(
        [FromServices] IResetPasswordUseCase useCase,
        [FromBody] RequestResetPasswordJson request)
    {
        await useCase.Execute(request);

        return NoContent();
    }
}