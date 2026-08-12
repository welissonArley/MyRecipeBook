using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Storage;

namespace MyRecipeBook.Infrastructure.Storage;

internal sealed class AzureStorageService : IStorageService
{
    private const string ProfilePictureFileName = "profile-picture";
    private const uint ProfilePictureExpirationInMinutes = 60;
    private const uint RecipeIllustrationExpirationInMinutes = 60;

    private readonly BlobServiceClient _blobServiceClient;

    public AzureStorageService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task UploadProfilePicture(User user, Stream file, string contentType)
    {
        await Upload(user.Id, file, ProfilePictureFileName, contentType);
    }

    public async Task UploadIllustration(Recipe recipe, Stream file, string contentType)
    {
        await Upload(recipe.UserId, file, recipe.Id.ToString(), contentType);
    }

    public string GetProfilePictureUrl(User user)
    {
        return GenerateReadUrl(user.Id, ProfilePictureFileName, ProfilePictureExpirationInMinutes);
    }

    public string GetIllustrationUrl(Recipe recipe)
    {
        return GenerateReadUrl(recipe.UserId, recipe.Id.ToString(), RecipeIllustrationExpirationInMinutes);
    }

    private async Task Upload(Guid userId, Stream file, string blobName, string contentType)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(userId.ToString());

        await containerClient.CreateIfNotExistsAsync();

        var blobClient = containerClient.GetBlobClient(blobName);

        await blobClient.UploadAsync(file, new BlobHttpHeaders
        {
            ContentType = contentType
        });
    }

    private string GenerateReadUrl(Guid userId, string blobName, uint expirationInMinutes)
    {
        var blob = _blobServiceClient
            .GetBlobContainerClient(userId.ToString())
            .GetBlobClient(blobName);

        return blob
            .GenerateSasUri(BlobSasPermissions.Read, DateTime.UtcNow.AddMinutes(expirationInMinutes))
            .ToString();
    }
}
