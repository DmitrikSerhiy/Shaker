using Blazored.LocalStorage;

namespace Shaker.Client.Services; 

public sealed class BarStateService {
    private readonly ILocalStorageService _localStorageService;
    public event Func<Task>? OnCurrentBarChange;
    public event Func<Task>? OnBarListChange;

    public static string CurrentBarName = "currentBar";
    private int? CurrentBar { get; set; }
    
    public BarStateService(ILocalStorageService localStorageService) {
        _localStorageService = localStorageService ?? throw new ArgumentNullException(nameof(localStorageService));
    }
    
    public async Task SetBarAsync(int barId) {
        CurrentBar = barId;
        await SaveToLocalStorage(CurrentBarName, barId);
        await NotifyCurrentBarStateChanged();
    }
    
    public async Task<int> GetBarAsync() {
        CurrentBar ??= await _localStorageService.GetItemAsync<int?>(CurrentBarName);
        return CurrentBar ?? -1;
    }

    public async Task RefreshBarListAsync() {
        await NotifyBarListStateChanged();
    }
    
    private Task NotifyCurrentBarStateChanged() => OnCurrentBarChange?.Invoke() ?? Task.CompletedTask;
    private Task NotifyBarListStateChanged() => OnBarListChange?.Invoke() ?? Task.CompletedTask;

    
    private async Task SaveToLocalStorage(string key, int value) {
        await _localStorageService.SetItemAsync(key, value);
    }
    
}