namespace Shaker; 

public class Cocktail {
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Recipe { get; set; } = null!;
    public List<Ingredient> Ingredients { get; set; } = new();
}