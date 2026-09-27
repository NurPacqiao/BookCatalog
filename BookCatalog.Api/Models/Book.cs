namespace BookCatalog.Api.Models;

public class Book
{
    public Guid Id { get; init; } = Guid.NewGuid();
    
    public required string Title { get; set; }
    public required string Author { get; set; }
    public required string Isbn { get; set; }
    public int PublicationYear { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAtUtc { get; init; } = DateTime.UtcNow;
}