using System.Text;
using System.Text.Json;
using Azure.Storage.Blobs;
using Shaker.Client.Common;
using Shaker.Client.Dtos;

namespace Shaker.Client.Services; 

public sealed class DataService {
    private readonly BlobServiceClient _blobServiceClient;

     public DataService(string connectionString) {
         _blobServiceClient = new BlobServiceClient(connectionString);
     }
    
    public async Task<List<Cocktail>> LoadCocktailsAsync() {
        var loadedCocktails = await GetJsonAsync<List<Cocktail?>>(Constants.CocktailsUrl);
        return loadedCocktails?.Where(c => c != null).Select(c => c!).ToList() ?? new List<Cocktail>();
    }
    
    public async Task<List<Ingredient>> LoadIngredientsAsync() {
        var loadedIngredients = await GetJsonAsync<List<Ingredient?>>(Constants.IngredientsUrl);
        return loadedIngredients?.Where(i => i != null).Select(i => i!).ToList() ?? new List<Ingredient>();
    }
    
    public async Task<Bar?> LoadBarAsync() {
        return await GetJsonAsync<Bar?>(Constants.BarUrl);
    }
    
    public async Task UpdateBarAsync(Bar bar) { 
        await UpdateJsonAsync(Constants.BarUrl, bar);
    }


    private async Task<T?> GetJsonAsync<T>(string jsonName) {
        var blobClient = _blobServiceClient.GetBlobContainerClient(Constants.ContainerName).GetBlobClient(jsonName);
        var response = await blobClient.DownloadContentAsync();
        return JsonSerializer.Deserialize<T>(response.Value.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
    
    private async Task UpdateJsonAsync<T>(string jsonName, T data) {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(Constants.ContainerName);
        var json = JsonSerializer.Serialize(data);
        var bytes = Encoding.UTF8.GetBytes(json);

        using var memoryStream = new MemoryStream(bytes);
        var blobClient = blobContainerClient.GetBlobClient(jsonName);
        await blobClient.UploadAsync(memoryStream, true);
    }
}