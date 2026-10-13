namespace BookCatalog.Domain.Entities;

public class Loan
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid BookId { get; set; }
    public Book Book { get; set; } = null!;
    public Guid MemberId { get; set; }
    public Member Member { get; set; } = null!;
    public DateTime BorrowedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? ReturnedAtUtc { get; set; }

    public bool IsOpen => ReturnedAtUtc is null;
}