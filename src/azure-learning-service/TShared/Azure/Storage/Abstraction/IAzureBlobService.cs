namespace TShared.Azure.Storage.Abstraction;

public interface IAzureBlobService
{
    Task UploadFileAsync(string containerName, string fileName, Stream stream);
    public string GetSasFileUrl(string containerName, string fileName);
}