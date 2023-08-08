using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using TShared.Azure.Storage.Abstraction;

namespace TShared.Azure.Storage;

public class AzureBlobService : IAzureBlobService
{
    private readonly BlobServiceClient _blobServiceClient;

    public AzureBlobService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task UploadFileAsync(string containerName, string fileName, Stream stream)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync();
        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(stream, true);
    }

    public string GetSasFileUrl(string containerName, string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(fileName);
        var userDelegationKey =  _blobServiceClient.GetUserDelegationKey(DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow.AddMinutes(2));
        
        var sasBuilder = new BlobSasBuilder()
        {
            BlobContainerName = blobClient.BlobContainerName,
            BlobName = blobClient.Name,
            Resource = "b", // b for blob, c for container
            StartsOn = DateTimeOffset.UtcNow,
            ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(2),
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read); // read permissions
        // Add the SAS token to the container URI.
        var blobUriBuilder = new BlobUriBuilder(blobClient.Uri)
        {
            Sas = sasBuilder.ToSasQueryParameters(userDelegationKey, _blobServiceClient.AccountName)
        };
        
        var uri = blobUriBuilder.ToUri().ToString();
        
        return uri;
    }
}