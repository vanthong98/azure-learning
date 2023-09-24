using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Serialization.HybridRow.Schemas;
using StackExchange.Redis;
using System.ComponentModel;
using PartitionKey = Microsoft.Azure.Cosmos.PartitionKey;

var databaseId = "ItemTests";
var clientEndPoint = @"https://vanthong98.documents.azure.com:443/";
var clientKey = "5IdlvJq66OEOPkj8OU1MWdEisFdLryJJbwkkNMOXWANj9iHOJsAEAYw6LRAnXqnDCfkaM8KJK9zwACDbEQFlEw==";
var cosmosClient = new CosmosClient(clientEndPoint, clientKey);
var dbResponse = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
var database = dbResponse.Database;

var containerId = "Items";
ContainerResponse simpleContainer = await database.CreateContainerIfNotExistsAsync(
    id: containerId,
    partitionKeyPath: "/Test",
    throughput: 400);


var container = simpleContainer.Container;

await container.CreateItemAsync(new
{
    id = Guid.NewGuid(),
}, PartitionKey.None);


await container.CreateItemAsync(new
{
    id = Guid.NewGuid(),
    Name = "Thông",
    Test = "Tao"
}, new PartitionKey("Tao"));

await container.DeleteContainerAsync();

await database.DeleteAsync();
Console.WriteLine("Hello");