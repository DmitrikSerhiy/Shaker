using Shaker.Client.Dtos;

namespace Shaker.Client.Services; 

public sealed class BarService {
    private readonly DataService _dataService;

    public BarService(DataService dataService) {
        _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
    }
    
    public async Task AddToBarAsync(Ingredient ingredient) {
        var bar = await _dataService.LoadBarAsync();
        if (bar == null) {
            return;
        }

        if (bar.Ingredients.Select(i => i.Id).Contains(ingredient.Id)) {
            return;
        }
        
        bar.Ingredients.Add(new Ingredient {Id = ingredient.Id});
        await _dataService.UpdateBarAsync(bar);
    }

    public async Task RemoveFromBarAsync(Ingredient ingredient) {
        var bar = await _dataService.LoadBarAsync();
        if (bar == null) {
            return;
        }

        if (!bar.Ingredients.Select(i => i.Id).Contains(ingredient.Id)) {
            return;
        }
        
        bar.Ingredients.RemoveAll(i => i.Id == ingredient.Id);
        await _dataService.UpdateBarAsync(bar);
    }
}