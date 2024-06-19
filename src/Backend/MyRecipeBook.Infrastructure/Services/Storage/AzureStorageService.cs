using Azure.Storage.Blobs;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Services.Storage;

namespace MyRecipeBook.Infrastructure.Services.Storage;
public class AzureStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;

    public AzureStorageService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task Upload(User user, Stream file, string fileName)
    {
        var container = _blobServiceClient.GetBlobContainerClient(user.UserIdentifier.ToString());
        await container.CreateIfNotExistsAsync();

        var blobClient = container.GetBlobClient(fileName);

        await blobClient.UploadAsync(file, overwrite: true);
    }
}