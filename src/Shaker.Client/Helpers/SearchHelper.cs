using Shaker.Client.Dtos;

namespace Shaker.Client.Helpers; 

public sealed class SearchHelper {
    public static List<Cocktail> Search(List<Cocktail> cocktails, string? query) {
        return string.IsNullOrWhiteSpace(query)
            ? cocktails
            : cocktails
                .Where(c =>
                    c.Name.Contains(query, StringComparison.InvariantCultureIgnoreCase) ||
                    c.Ingredients.Any(i => i.Name.Contains(query, StringComparison.InvariantCultureIgnoreCase)))
                .ToList();
    }
}