namespace Shaker.Client.Dtos; 

public sealed record Profile {
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}