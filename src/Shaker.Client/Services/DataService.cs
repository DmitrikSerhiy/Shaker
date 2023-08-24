using Shaker.Client.Common;
using Shaker.Client.Dtos;

namespace Shaker.Client.Services; 

public sealed class DataService {
    private readonly IRepository _repository;

    private List<Cocktail>? _cachedCocktails;
    private List<Ingredient>? _cachedIngredients;
    private Bar? _cachedBar;


    public DataService(IRepository repository) {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    
    public async Task<List<Cocktail>> LoadCocktailsAsync() {
        if (_cachedCocktails != null)
        {
            return _cachedCocktails;
        }
        
        var loadedCocktails = await _repository.GetData<List<Cocktail?>>(ShakerConstants.CocktailsUrl);
        _cachedCocktails = loadedCocktails?.Where(c => c != null).Select(c => c!).ToList() ?? new List<Cocktail>();
        _cachedCocktails.Sort();
        return _cachedCocktails;
    }
    
    public async Task<List<Ingredient>> LoadIngredientsAsync() {
        if (_cachedIngredients != null)
        {
            return _cachedIngredients;
        }
        
        var loadedIngredients = await _repository.GetData<List<Ingredient?>>(ShakerConstants.IngredientsUrl);
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
    
    public async Task<Bar?> LoadBarAsync(int barId) {
        if (_cachedBar != null) {
            return _cachedBar;
        }

        if (barId == -1) {
            return null;
        }
        
        _cachedBar = await _repository.GetData<Bar?>(GetBarUrl(barId));
        return _cachedBar;
    }
    
    public async Task UpdateBarAsync(Bar bar, int barId) {
        await _repository.UpdateData(GetBarUrl(barId), bar);
        _cachedBar = null;
    }
    
    public void ClearCachedBar() {
        _cachedBar = null;
    }

    public async Task<List<Profile>> LoadProfilesAsync() {
        var profiles = await _repository.GetData<List<Profile>>(ShakerConstants.ProfilesUrl);
        return profiles ?? new List<Profile>();
    }

    public async Task UpdateProfilesAsync(List<Profile> profiles) {
        await _repository.UpdateData(ShakerConstants.ProfilesUrl, profiles);
    }

    public async Task CreateBarAsync(Bar bar) {
        await _repository.AddDataAsync($"bar_{bar.Id}", bar);
    }

    private string GetBarUrl(int barId) {
        var urlParts = ShakerConstants.BarUrl.Split('.');
        return $"{urlParts[0]}_{barId}.{urlParts[1]}"; 
    }
}