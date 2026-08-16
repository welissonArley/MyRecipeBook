using Moq;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Storage;

namespace CommonTestUtilities.Storage;

public class IStorageServiceBuilder
{
    public const string FakeUrl = "https://fake-storage.com/image";

    public static IStorageService Build()
    {
        var mock = new Mock<IStorageService>();

        mock.Setup(storage => storage.GetProfilePictureUrl(It.IsAny<User>())).Returns(FakeUrl);
        mock.Setup(storage => storage.GetRecipeIllustrationUrl(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(FakeUrl);

        return mock.Object;
    }
}