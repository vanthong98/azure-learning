
using Azure.Identity;
using Azure.Storage.Blobs;
using TShared.Azure.Storage;


await TestAzureBlobService();

async Task TestAzureBlobService()
{
    const string storageDomain = "https://vanthong98.blob.core.windows.net";
    var blobServiceClient = new BlobServiceClient(new Uri(storageDomain), new DefaultAzureCredential());
    var azureBlobService = new AzureBlobService(blobServiceClient);
}