namespace Shaker.Client.Dtos; 

public sealed record Bar {
    public List<Ingredient> Ingredients { get; set; } = new();

}