using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Azure.Storage.Blobs;
using Shaker.Client.Common;
using Shaker.Client.Dtos;

namespace Shaker.Client.Services; 

public sealed class DataService {
    private readonly BlobServiceClient _blobServiceClient;
    
    private List<Cocktail>? _cachedCocktails;
    private List<Ingredient>? _cachedIngredients;

     public DataService(string connectionString) {
         _blobServiceClient = new BlobServiceClient(connectionString);
     }
    
    public async Task<List<Cocktail>> LoadCocktailsAsync() {
        if (_cachedCocktails != null)
        {
            return _cachedCocktails;
        }
        
        var loadedCocktails = await GetJsonAsync<List<Cocktail?>>(Constants.CocktailsUrl);
        _cachedCocktails = loadedCocktails?.Where(c => c != null).Select(c => c!).ToList() ?? new List<Cocktail>();
        _cachedCocktails.Sort();
        return _cachedCocktails;
    }
    
    public async Task<List<Ingredient>> LoadIngredientsAsync() {
        if (_cachedIngredients != null)
        {
            return _cachedIngredients;
        }
        
        var loadedIngredients = await GetJsonAsync<List<Ingredient?>>(Constants.IngredientsUrl);
        _cachedIngredients = loadedIngredients?.Where(i => i != null).Select(i => i!).ToList() ?? new List<Ingredient>();
        _cachedIngredients.Sort();
        return _cachedIngredients;
    }

    public async Task SetCocktailsAndIngredientsCache() {
        var tasks = new List<Task> {
            LoadCocktailsAsync(),
            LoadIngredientsAsync()
        };

        await Task.WhenAll(tasks);
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
        var serializeOptions = new JsonSerializerOptions(JsonSerializerOptions.Default)
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        var json = JsonSerializer.Serialize(data, serializeOptions);
        var bytes = Encoding.UTF8.GetBytes(json);

        using var memoryStream = new MemoryStream(bytes);
        var blobClient = blobContainerClient.GetBlobClient(jsonName);
        await blobClient.UploadAsync(memoryStream, true);
    }
}