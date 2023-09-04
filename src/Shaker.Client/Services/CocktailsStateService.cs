using Blazored.LocalStorage;

namespace Shaker.Client.Services; 

public sealed class CocktailsStateService {
    private readonly ILocalStorageService _localStorageService;
    public event Func<Task>? OnCocktailsChange;
    public event Func<Task>? OnIngredientsChange;

    public static string AllCocktailsCountName = "allCocktailsCount";
    public static string AvailableCocktailsCountName = "availableCocktailsCount";
    public static string IngredientsCountName = "ingredientsCount";

    public CocktailsStateService(ILocalStorageService localStorageService) {
        _localStorageService = localStorageService ?? throw new ArgumentNullException(nameof(localStorageService));
    }
    
    private int? AllCocktailsCount { get; set; }
    private int? AvailableCocktailsCount { get; set; }
    
    private int? IngredientsCount { get; set; }

    public async Task SetCalculatedCocktailsCountsAsync(int availableCocktailsCount, int ingredientsCount) {
        await SetAvailableCocktailsCountAsync(availableCocktailsCount);
        await SetIngredientsCountAsync(ingredientsCount);
    }
    
    
    public async Task SetAllCocktailsCountAsync(int count) {
        AllCocktailsCount = count;
        await SaveToLocalStorage(AllCocktailsCountName, count);
        await NotifyCocktailsStateChanged();
    }

    public async Task SetAvailableCocktailsCountAsync(int count) {
        AvailableCocktailsCount = count;
        await SaveToLocalStorage(AvailableCocktailsCountName, count);
        await NotifyCocktailsStateChanged();
    }
    
    public async Task SetIngredientsCountAsync(int count) {
        IngredientsCount = count;
        await SaveToLocalStorage(IngredientsCountName, count);
        await NotifyIngredientsStateChanged();
    }
    
    
    
    public async Task<int> GetAllCocktailsCountAsync() {
        AllCocktailsCount ??= await _localStorageService.GetItemAsync<int>(AllCocktailsCountName);
        return AllCocktailsCount.GetValueOrDefault();
    }
    
    public async Task<int> GetAvailableCocktailsCountAsync() {
        AvailableCocktailsCount ??= await _localStorageService.GetItemAsync<int>(AvailableCocktailsCountName);
        return AvailableCocktailsCount.GetValueOrDefault();
    }
    
    public async Task<int> GetIngredientsCountAsync() {
        IngredientsCount ??= await _localStorageService.GetItemAsync<int>(IngredientsCountName);
        return IngredientsCount.GetValueOrDefault();
    }

    private Task NotifyCocktailsStateChanged() => OnCocktailsChange?.Invoke() ?? Task.CompletedTask;
    private Task NotifyIngredientsStateChanged() => OnIngredientsChange?.Invoke() ?? Task.CompletedTask;


    private async Task SaveToLocalStorage(string key, int value) {
        await _localStorageService.SetItemAsync(key, value);
    }
}