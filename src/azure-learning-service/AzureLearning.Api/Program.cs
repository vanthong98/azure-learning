using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Core.Implementations;
using StackExchange.Redis.Extensions.System.Text.Json;
using TShared;
using TShared.Azure.Storage;
using TShared.Azure.Storage.Abstraction;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddSingleton<IAzureBlobService, AzureBlobService>();

const string storageUrl = "https://vanthong98.blob.core.windows.net";

services.AddAzureClientServices(storageUrl);

services.AddSingleton<IRedisClientFactory, RedisClientFactory>();
services.AddSingleton<ISerializer, SystemTextJsonSerializer>();

services.AddSingleton((provider) => provider
    .GetRequiredService<IRedisClientFactory>()
    .GetDefaultRedisClient());

services.AddSingleton((provider) => provider
    .GetRequiredService<IRedisClientFactory>()
    .GetDefaultRedisClient()
    .GetDefaultDatabase());

var redisConfiguration = configuration.GetSection("Redis").Get<RedisConfiguration>();

if (redisConfiguration != null)
{
    services.AddSingleton(redisConfiguration);
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
