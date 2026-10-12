namespace BookCatalog.Domain.Entities;

public class Book
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Title { get; set; }
    public required string Isbn { get; set; }
    public int PublicationYear { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAtUtc { get; init; } = DateTime.UtcNow;

    public Guid AuthorId { get; set; }
    public Author Author { get; set; } = null!;

    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}