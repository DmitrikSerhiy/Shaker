using Shaker.Client.Dtos;

namespace Shaker.Client.Helpers; 

public sealed class SearchHelper {
    public static List<Cocktail> SearchCocktails(List<Cocktail> cocktails, string? query) {
        return string.IsNullOrWhiteSpace(query)
            ? cocktails
            : cocktails
                .Where(c =>
                    c.Name.Contains(query, StringComparison.InvariantCultureIgnoreCase) ||
                    c.Ingredients.Any(i => i.Name.Contains(query, StringComparison.InvariantCultureIgnoreCase)))
                .ToList();
    }
    
    public static List<Ingredient> SearchIngredients(List<Ingredient> ingredients, string? query) {
        return string.IsNullOrWhiteSpace(query)
            ? ingredients
            : ingredients
                .Where(c =>
                    c.Name.Contains(query, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
    }
}