using Shaker.Client.Dtos;

namespace Shaker.Client.Services; 

public sealed class CocktailService {
    public List<Cocktail> GetAvailableCocktails(Bar bar, List<Cocktail> knownCocktails)
    {
        var cocktails = knownCocktails.Where(cocktail => cocktail.Ingredients.All(bar.Ingredients.Contains)).ToList();
        cocktails.Sort();
        return cocktails.ToList();
    }
    
    public List<Cocktail> MapAndSortFavoriteCocktails(Bar bar, List<Cocktail> cocktails)
    {
        foreach (var cocktail in cocktails) {
            cocktail.IsFavorite = bar.FavoriteCocktails?.Contains(cocktail.Id) ?? false;
        }
        return cocktails.OrderByDescending(c => c.IsFavorite).ThenBy(c => c.Name).ToList();
    }
}