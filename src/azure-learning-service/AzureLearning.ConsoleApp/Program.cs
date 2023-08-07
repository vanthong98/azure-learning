
using TShared.Azure.ServiceBus;
using TShared.Azure.Storage;

var blobService = new BlobService();
await blobService.RunAsync();
Console.WriteLine("DONE");