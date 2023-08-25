using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Shaker.Client.Common;

namespace Shaker.Client.Services; 

public sealed class BlobRepository : IRepository {
    
    private readonly BlobServiceClient _blobServiceClient;
    
    private string Name => "myshaker";
    private string Protocol => "https";
    private string ByteKey =>
        "108 52 77 78 117 76 97 118 67 79 97 108 54 89 53 72 110 107 97 98 83 89 105 98 88 111 51 110 54 77 50 71 117 111 87 108 65 74 49 67 102 90 114 118 107 76 57 112 70 49 50 66 65 120 65 105 85 84 55 71 79 108 51 86 103 43 121 49 75 65 106 67 104 121 67 113 43 65 83 116 76 105 100 49 85 81 61 61";

    public BlobRepository() {
        var bytes = ByteKey.Split(' ').Select(s => byte.Parse(s)).ToArray();
        var key = Encoding.UTF8.GetString(bytes);
        _blobServiceClient = new BlobServiceClient($"DefaultEndpointsProtocol={Protocol};AccountName={Name};AccountKey={key};EndpointSuffix=core.windows.net");
    }
    
    public async Task<T?> GetData<T>(string jsonName) {
        var blobClient = _blobServiceClient.GetBlobContainerClient(ShakerConstants.ContainerName).GetBlobClient(jsonName);
        var response = await blobClient.DownloadContentAsync();
        return JsonSerializer.Deserialize<T>(response.Value.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
    
    public async Task UpdateData<T>(string jsonName, T data) {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(ShakerConstants.ContainerName);
        var serializeOptions = new JsonSerializerOptions(JsonSerializerOptions.Default)
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true
        };
        var json = JsonSerializer.Serialize(data, serializeOptions);
        var bytes = Encoding.UTF8.GetBytes(json);

        using var memoryStream = new MemoryStream(bytes);
        var blobClient = blobContainerClient.GetBlobClient(jsonName);
        await blobClient.UploadAsync(memoryStream, true);
    }

    public async Task AddDataAsync<T>(string jsonName, T data) {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(ShakerConstants.ContainerName);
        var serializeOptions = new JsonSerializerOptions(JsonSerializerOptions.Default)
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true
        };

        var json = JsonSerializer.Serialize(data, serializeOptions);
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        
        var uploadOptions = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders
            {
                ContentType = "application/json;charset=utf-8"
            },
            AccessTier = AccessTier.Hot
        };
        
        var blobClient = blobContainerClient.GetBlobClient(jsonName);
        await blobClient.UploadAsync(stream, uploadOptions);
    }

    public async Task DeleteDataAsync(string jsonName) {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(ShakerConstants.ContainerName);
        var blobClient = blobContainerClient.GetBlobClient(jsonName);
        await blobClient.DeleteAsync(DeleteSnapshotsOption.IncludeSnapshots);
    }
}