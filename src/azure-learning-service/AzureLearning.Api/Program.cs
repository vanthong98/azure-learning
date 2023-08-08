using Azure.Identity;
using Microsoft.Extensions.Azure;
using TShared.Azure.Storage;
using TShared.Azure.Storage.Abstraction;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IAzureBlobService, AzureBlobService>();

const string storageUrl = "https://vanthong98.blob.core.windows.net";

builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(new Uri(storageUrl));
    clientBuilder.UseCredential(new DefaultAzureCredential());
});

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
