using Bogus;
using Moq;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Services.Storage;

namespace CommonTestUtilities.BlobStorage;
public class BlobStorageServiceBuilder
{
    private readonly Mock<IBlobStorageService> _mock;

    public BlobStorageServiceBuilder() => _mock = new Mock<IBlobStorageService>();

    public BlobStorageServiceBuilder GetFileUrl(User user, string? fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return this;

        var faker = new Faker();
        var imageUrl = faker.Image.LoremPixelUrl();

        _mock.Setup(blobStorage => blobStorage.GetFileUrl(user, fileName)).ReturnsAsync(imageUrl);

        return this;
    }

    public BlobStorageServiceBuilder GetFileUrl(User user, IList<Recipe> recipes)
    {
        foreach(var recipe in recipes)
        {
            GetFileUrl(user, recipe.ImageIdentifier);
        }

        return this;
    }

    public IBlobStorageService Build() => _mock.Object;
}
