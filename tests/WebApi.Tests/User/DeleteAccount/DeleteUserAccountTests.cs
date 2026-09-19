using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Net;
using WebApi.Tests.Resources;

namespace WebApi.Tests.User.DeleteAccount;

public class DeleteUserAccountTests : BaseIntegrationTest<ServiceBusApplicationFactory>
{
    private const string REQUEST_URI = "users";

    private readonly UserIdentityManager _user1;

    public DeleteUserAccountTests(ServiceBusApplicationFactory factory) : base(factory)
    {
        _user1 = factory.User1;
    }

    [Fact]
    public async Task Success()
    {
        var response = await Delete(REQUEST_URI, _user1.GetAccessToken());

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await WaitUntilAccountIsDeleted(_user1.GetId());

        var recipesExist = await DbContext.Recipes.AnyAsync(recipe => recipe.UserId == _user1.GetId());
        recipesExist.ShouldBeFalse();

        var verificationCodesExist = await DbContext.VerificationCodes.AnyAsync(code => code.UserId == _user1.GetId());
        verificationCodesExist.ShouldBeFalse();
    }

    private async Task WaitUntilAccountIsDeleted(Guid userId)
    {
        for (var attempt = 0; attempt < 20; attempt++)
        {
            var accountExists = await DbContext.Users.AnyAsync(user => user.Id == userId);
            if (accountExists == false)
                return;

            await Task.Delay(500);
        }

        throw new TimeoutException("A conta não foi removida a tempo.");
    }
}
