namespace Shaker.Client.Dtos; 

public sealed record Bar {
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public List<Ingredient> Ingredients { get; set; } = new();
    public List<int>? FavoriteCocktails { get; set; } = new();
    public List<int>? HiddenCocktails { get; set; } = new();
}