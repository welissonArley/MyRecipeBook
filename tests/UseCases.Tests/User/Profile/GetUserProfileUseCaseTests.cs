using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Storage;
using MyRecipeBook.Application.UseCases.User.Profile;
using Shouldly;

namespace UseCases.Tests.User.Profile;

public class GetUserProfileUseCaseTests
{
    [Theory]
    [InlineData(true, IStorageServiceBuilder.FakeUrl)]
    [InlineData(false, "")]
    public async Task Success(bool hasImage, string expectedUrl)
    {
        (var user, var _) = UserBuilder.Build();
        user.HasImage = hasImage;

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute();

        result.ShouldNotBeNull();
        result.Name.ShouldBe(user.Name);
        result.Email.ShouldBe(user.Email);
        result.ImageUrl.ShouldBe(expectedUrl);
    }

    private static GetUserProfileUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user)
    {
        var loggedUser = ILoggedUserBuilder.Build(user);
        var storageService = IStorageServiceBuilder.Build();

        return new GetUserProfileUseCase(loggedUser, storageService);
    }
}
