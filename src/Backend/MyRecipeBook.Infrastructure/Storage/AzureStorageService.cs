using Azure.Storage.Blobs;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Storage;

namespace MyRecipeBook.Infrastructure.Storage;

internal sealed class AzureStorageService : IStorageService
{
    private readonly BlobServiceClient _blobServiceClient;

    public AzureStorageService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public Task UploadProfilePicture(User user, Stream file, string contentType)
    {
        throw new NotImplementedException();
    }

    public Task UploadIllustration(Recipe recipe, Stream file, string contentType)
    {
        throw new NotImplementedException();
    }
}
