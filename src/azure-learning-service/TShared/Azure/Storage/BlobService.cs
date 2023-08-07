using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Storage.Blobs;

namespace TShared.Azure.Storage
{
    public class BlobService
    {
        public async Task RunAsync()
        {
            var storageDomain = "https://vanthong98.blob.core.windows.net";
            var blobServiceClient = new BlobServiceClient(new Uri(storageDomain), new DefaultAzureCredential());
            var containerName = "quickstartblobs" + Guid.NewGuid();
            BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);

            var localPath = "data";
            Directory.CreateDirectory(localPath);
            var fileName = "quickstart" + Guid.NewGuid() + ".txt";
            var localFilePath = Path.Combine(localPath, fileName);
            await File.WriteAllTextAsync(localFilePath, "Hello, World!");
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);
            await blobClient.UploadAsync(localFilePath, true);
        }
    }
}
