namespace Shaker.Client.Dtos;

using Helpers;


public sealed record Ingredient : IComparable<Ingredient> {
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public IngredientGroup Group { get; set; }
    public int CompareTo(Ingredient? other) {
        return other == null ? 1 : string.Compare(Name, other.Name, StringComparison.InvariantCultureIgnoreCase);
    }
}