using Shaker.Client.Dtos;

namespace Shaker.Client.Services; 

public sealed class CocktailService {
    public IEnumerable<Cocktail> GetAvailableCocktails(List<Ingredient> availableIngredients, List<Cocktail> allCocktails)
    {
        return allCocktails.Where(cocktail => cocktail.Ingredients.All(availableIngredients.Contains));
    }
}