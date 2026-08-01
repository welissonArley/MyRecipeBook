using CommonTestUtilities.Requests;
using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Tests.InlineData;
using WebApi.Tests.Resources;

namespace WebApi.Tests.PasswordRecovery.ResetPassword;

public class ResetPasswordTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "authentication/password-recovery/reset";

    private readonly UserIdentityManager _user1;

    public ResetPasswordTests(MyRecipeBookApplicationFactory factory) : base(factory)
    {
        _user1 = factory.User1;
    }

    [Fact]
    public async Task Success()
    {
        var verificationCode = new VerificationCode
        {
            Code = "123456",
            Type = VerificationCodeType.PasswordRecovery,
            UserId = _user1.GetId()
        };

        await DbContext.VerificationCodes.AddAsync(verificationCode);
        await DbContext.SaveChangesAsync();

        var request = new RequestResetPasswordJson
        {
            Email = _user1.GetEmail(),
            Code = verificationCode.Code,
            NewPassword = "new-password-123"
        };

        var response = await Post(REQUEST_URI, request);

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var responseBody = await response.Content.ReadAsStringAsync();
        responseBody.ShouldBeEmpty();

        var codeExists = await DbContext.VerificationCodes.AnyAsync(code => code.Id == verificationCode.Id);
        codeExists.ShouldBeFalse();
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task ShouldThrowError_WhenUserDoesNotExist(string culture)
    {
        var request = RequestResetPasswordJsonBuilder.Build();

        var response = await Post(REQUEST_URI, request, culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("VERIFICATION_CODE_INVALID", new CultureInfo(culture));

        errors.ShouldSatisfyAllConditions(errorsList =>
        {
            errorsList.Count().ShouldBe(1);
            errorsList.ShouldContain(error => error.GetString().IsNotEmpty() && error.GetString()!.Equals(expectedMessage));
        });
    }
}
