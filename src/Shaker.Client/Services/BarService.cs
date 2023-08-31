using Shaker.Client.Dtos;

namespace Shaker.Client.Services; 

public sealed class BarService {
    private readonly DataService _dataService;

    public BarService(DataService dataService) {
        _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
    }
    
    public async Task AddToBarAsync(Ingredient ingredient, int barId) {
        var bar = await _dataService.LoadBarAsync(barId) ?? new Bar();

        if (bar.Ingredients.Select(i => i.Id).Contains(ingredient.Id)) {
            return;
        }
        
        bar.Ingredients.Add(new Ingredient {Id = ingredient.Id});
        SelectIds(bar);
        await _dataService.UpdateBarAsync(bar, barId);
    }

    public async Task RemoveFromBarAsync(Ingredient ingredient, int barId) {
        var bar = await _dataService.LoadBarAsync(barId) ?? new Bar();

        if (!bar.Ingredients.Select(i => i.Id).Contains(ingredient.Id)) {
            return;
        }
        
        bar.Ingredients.RemoveAll(i => i.Id == ingredient.Id);
        SelectIds(bar);
        await _dataService.UpdateBarAsync(bar, barId);
    }
    
    public async Task AddToFavoritesAsync(int cocktailId, int barId) {
        var bar = await _dataService.LoadBarAsync(barId) ?? new Bar();

        if (bar.FavoriteCocktails?.Contains(cocktailId) ?? false) {
            return;
        }
        
        bar.FavoriteCocktails ??= new List<int>();
        bar.FavoriteCocktails.Add(cocktailId);
        SelectIds(bar);
        await _dataService.UpdateBarAsync(bar, barId);
    }
    
    public async Task RemoveFromFavoritesAsync(int cocktailId, int barId) {
        var bar = await _dataService.LoadBarAsync(barId) ?? new Bar();

        if (!bar.FavoriteCocktails?.Contains(cocktailId) ?? true) {
            return;
        }
        
        bar.FavoriteCocktails.RemoveAll(c => c == cocktailId);
        SelectIds(bar);
        await _dataService.UpdateBarAsync(bar, barId);
    }
    
    public async Task HideCocktailAsync(int cocktailId, int barId) {
        var bar = await _dataService.LoadBarAsync(barId) ?? new Bar();

        if (bar.HiddenCocktails?.Contains(cocktailId) ?? false) {
            return;
        }
        
        bar.HiddenCocktails ??= new List<int>();
        bar.HiddenCocktails.Add(cocktailId);
        SelectIds(bar);
        await _dataService.UpdateBarAsync(bar, barId);
    }
    
    public async Task UnHideCocktailAsync(int cocktailId, int barId) {
        var bar = await _dataService.LoadBarAsync(barId) ?? new Bar();

        if (!bar.HiddenCocktails?.Contains(cocktailId) ?? true) {
            return;
        }
        
        bar.HiddenCocktails.RemoveAll(c => c == cocktailId);
        SelectIds(bar);
        await _dataService.UpdateBarAsync(bar, barId);
    }
    
    

    private void SelectIds(Bar bar) {
        bar.Ingredients = bar.Ingredients.Select(i => new Ingredient { Id = i.Id }).ToList();
        bar.FavoriteCocktails = bar.FavoriteCocktails?.ToList();
        bar.HiddenCocktails = bar.HiddenCocktails?.ToList();
    }
}