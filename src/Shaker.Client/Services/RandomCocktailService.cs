namespace Shaker.Client.Services; 

public sealed class RandomCocktailService {
    public event Action? OnRandomCocktailButtonClicked;
    public event Func<int, Task>? OnRandomCocktailPopupOpened; 

    public void RandomCocktailButtonClicked() {
        OnRandomCocktailButtonClicked?.Invoke();
    }
    
    public void OpenRandomCocktailPopup(int cocktailId) {
        OnRandomCocktailPopupOpened?.Invoke(cocktailId);
    }
}