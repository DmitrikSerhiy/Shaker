using Shaker.Client.Dtos;

namespace Shaker.Client.Services; 

public sealed class CocktailService {
    public IEnumerable<Cocktail> GetAvailableCocktails(List<Ingredient> availableIngredients, List<Cocktail> knownCocktails)
    {
        return knownCocktails.Where(cocktail => cocktail.Ingredients.All(availableIngredients.Contains));
    }
}