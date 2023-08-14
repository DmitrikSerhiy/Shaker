using Shaker.Client.Dtos;

namespace Shaker.Client.Services; 

public sealed class BarService {
    private readonly DataService _dataService;

    public BarService(DataService dataService) {
        _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
    }
    
    public async Task AddToBarAsync(Ingredient ingredient) {
        var bar = await _dataService.LoadBarAsync() ?? new Bar();

        if (bar.Ingredients.Select(i => i.Id).Contains(ingredient.Id)) {
            return;
        }
        
        bar.Ingredients.Add(new Ingredient {Id = ingredient.Id});
        SelectIds(bar);
        await _dataService.UpdateBarAsync(bar);
    }

    public async Task RemoveFromBarAsync(Ingredient ingredient) {
        var bar = await _dataService.LoadBarAsync() ?? new Bar();

        if (!bar.Ingredients.Select(i => i.Id).Contains(ingredient.Id)) {
            return;
        }
        
        bar.Ingredients.RemoveAll(i => i.Id == ingredient.Id);
        SelectIds(bar);
        await _dataService.UpdateBarAsync(bar);
    }
    
    public async Task AddToFavoritesAsync(int cocktailId) {
        var bar = await _dataService.LoadBarAsync() ?? new Bar();

        if (bar.FavoriteCocktails?.Contains(cocktailId) ?? false) {
            return;
        }
        
        bar.FavoriteCocktails ??= new List<int>();
        bar.FavoriteCocktails.Add(cocktailId);
        SelectIds(bar);
        await _dataService.UpdateBarAsync(bar);
    }
    
    
    public async Task RemoveFromFavoritesAsync(int cocktailId) {
        var bar = await _dataService.LoadBarAsync() ?? new Bar();

        if (!bar.FavoriteCocktails?.Contains(cocktailId) ?? true) {
            return;
        }
        
        bar.FavoriteCocktails.RemoveAll(c => c == cocktailId);
        SelectIds(bar);
        await _dataService.UpdateBarAsync(bar);
    }

    private void SelectIds(Bar bar) {
        bar.Ingredients = bar.Ingredients.Select(i => new Ingredient { Id = i.Id }).ToList();
        bar.FavoriteCocktails = bar.FavoriteCocktails?.ToList();
    }
}