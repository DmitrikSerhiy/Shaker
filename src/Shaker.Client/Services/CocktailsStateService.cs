using Blazored.LocalStorage;

namespace Shaker.Client.Services; 

public sealed class CocktailsStateService {
    private readonly ILocalStorageService _localStorageService;
    public event Func<Task>? OnChange;

    public static string AllCocktailsCountName = "allCocktailsCount";
    public static string AvailableCocktailsCountName = "availableCocktailsCount";

    public CocktailsStateService(ILocalStorageService localStorageService) {
        _localStorageService = localStorageService ?? throw new ArgumentNullException(nameof(localStorageService));
    }
    
    private int? AllCocktailsCount { get; set; }
    private int? AvailableCocktailsCount { get; set; }

        public async Task SetAllCocktailsCountAsync(int count) {
            AllCocktailsCount = count;
            await SaveToLocalStorage(AllCocktailsCountName, count);
            await NotifyStateChanged();
        }

    public async Task SetAvailableCocktailsCountAsync(int count) {
        AvailableCocktailsCount = count;
        await SaveToLocalStorage(AvailableCocktailsCountName, count);
        await NotifyStateChanged();
    }
    
    public async Task<int> GetAllCocktailsCountAsync() {
        AllCocktailsCount ??= await _localStorageService.GetItemAsync<int>(AllCocktailsCountName);
        return AllCocktailsCount.GetValueOrDefault();
    }
    
    public async Task<int> GetAvailableCocktailsCountAsync() {
        AvailableCocktailsCount ??= await _localStorageService.GetItemAsync<int>(AvailableCocktailsCountName);
        return AvailableCocktailsCount.GetValueOrDefault();
    }

    private Task NotifyStateChanged() => OnChange?.Invoke() ?? Task.CompletedTask;

    private async Task SaveToLocalStorage(string key, int value) {
        await _localStorageService.SetItemAsync(key, value);
    }
}