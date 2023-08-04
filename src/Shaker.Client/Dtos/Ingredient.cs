﻿namespace Shaker.Client.Dtos; 

public sealed record Ingredient {
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}