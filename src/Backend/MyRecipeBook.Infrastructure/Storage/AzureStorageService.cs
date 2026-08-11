using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Storage;

namespace MyRecipeBook.Infrastructure.Storage;

internal sealed class AzureStorageService : IStorageService
{
    private const string ProfilePicturesContainerName = "profile-picture";

    private readonly BlobServiceClient _blobServiceClient;

    public AzureStorageService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task UploadProfilePicture(User user, Stream file, string contentType)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(user.Id.ToString());

        await containerClient.CreateIfNotExistsAsync();

        var blobClient = containerClient.GetBlobClient(ProfilePicturesContainerName);

        await blobClient.UploadAsync(file, new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders
            {
                ContentType = contentType
            }
        });
    }

    public async Task UploadIllustration(Recipe recipe, Stream file, string contentType)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(recipe.UserId.ToString());

        await containerClient.CreateIfNotExistsAsync();

        var blobClient = containerClient.GetBlobClient(recipe.Id.ToString());

        await blobClient.UploadAsync(file, new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders
            {
                ContentType = contentType
            }
        });
    }
}
