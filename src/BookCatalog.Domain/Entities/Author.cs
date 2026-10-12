namespace BookCatalog.Domain.Entities;

public class Author
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; set; }
    public ICollection<Book> Books { get; set; } = new List<Book>();
}