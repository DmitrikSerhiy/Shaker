namespace Shaker; 

public sealed class CocktailService {
    private readonly Bar _bar;

    public CocktailService(Bar bar)
    {
        _bar = bar;
    }

    public IEnumerable<Cocktail> GetPossibleCocktails()
    {
        return _bar.AvailableCocktails
            .Where(c => c.Ingredients.All(i => _bar.AvailableIngredients.Contains(i)));
    }
}