using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using MyRecipeBook.Application.UseCases.User.Profile;
using Shouldly;

namespace UseCases.Tests.User.Profile;

public class GetUserProfileUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        (var user, var _) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute();

        result.ShouldNotBeNull();
        result.Name.ShouldBe(user.Name);
        result.Email.ShouldBe(user.Email);
    }

    private static GetUserProfileUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user)
    {
        var loggedUser = ILoggedUserBuilder.Build(user);

        return new GetUserProfileUseCase(loggedUser);
    }
}
