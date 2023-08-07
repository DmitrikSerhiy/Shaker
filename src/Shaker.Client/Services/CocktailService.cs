using Shaker.Client.Dtos;

namespace Shaker.Client.Services; 

public sealed class CocktailService {
    public IEnumerable<Cocktail> GetAvailableCocktails(List<Ingredient> availableIngredients, List<Cocktail> knownCocktails)
    {
        var cocktails = knownCocktails.Where(cocktail => cocktail.Ingredients.All(availableIngredients.Contains)).ToList();
        cocktails.Sort();
        return cocktails;
    }
}