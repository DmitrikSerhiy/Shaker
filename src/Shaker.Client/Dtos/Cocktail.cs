﻿using Shaker.Client.Helpers;

namespace Shaker.Client.Dtos; 

public sealed record Cocktail : IComparable<Cocktail> {
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Recipe { get; set; } = null!;
    public CocktailType Type { get; set; }
    public bool IsClassic { get; set; }
    public bool IsMine { get; set; }
    public List<Ingredient> Ingredients { get; set; } = new();
    public int CompareTo(Cocktail? other) {
        return other == null ? 1 : string.Compare(Name, other.Name, StringComparison.InvariantCultureIgnoreCase);
    }
}