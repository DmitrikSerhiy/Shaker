using Shaker.Client.Dtos;

namespace Shaker.Client.Helpers; 

public sealed class SearchHelper {
    
    
    public static IQueryable<Cocktail> FilterCocktailsByName(IQueryable<Cocktail> cocktails, string? query) {
        return string.IsNullOrWhiteSpace(query)
            ? cocktails.AsQueryable()
            : cocktails
                .Where(c =>
                    c.Name.Contains(query, StringComparison.InvariantCultureIgnoreCase) ||
                    c.Ingredients.Any(i => i.Name.Contains(query, StringComparison.InvariantCultureIgnoreCase)))
                .AsQueryable();
    }
    
    public static IQueryable<Cocktail> FilterCocktailsBySource(IQueryable<Cocktail> cocktails, int source) {
        cocktails = source switch
        {
            1 => cocktails.Where(cocktail => cocktail.IsClassic),
            2 => cocktails.Where(cocktail => cocktail.IsMine),
            _ => cocktails
        };

        return cocktails;
    }
    public static IQueryable<Cocktail> FilterCocktailsByType(IQueryable<Cocktail> cocktails, CocktailType type) {
        cocktails = type switch
        {
            CocktailType.LongDrink => cocktails.Where(cocktail => cocktail.Type == CocktailType.LongDrink),
            CocktailType.Shot => cocktails.Where(cocktail => cocktail.Type == CocktailType.Shot),
            _ => cocktails
        };

        return cocktails;
    }


    public static List<Cocktail> SearchCocktails(List<Cocktail> cocktails, string? query) {
        return FilterCocktailsByName(cocktails.AsQueryable(), query).ToList();
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