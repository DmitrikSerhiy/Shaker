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
    
    public async Task<Bar?> LoadBarAsync() {
        if (_cachedBar != null) {
            return _cachedBar;
        }
        _cachedBar = await _repository.GetData<Bar?>(ShakerConstants.BarUrl);
        return _cachedBar;
    }
    
    public async Task UpdateBarAsync(Bar bar) {
        await _repository.UpdateData(ShakerConstants.BarUrl, bar);
        _cachedBar = null;
    }


}