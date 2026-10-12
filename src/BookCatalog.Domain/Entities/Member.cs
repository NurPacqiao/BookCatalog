namespace BookCatalog.Domain.Entities;

public class Member
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string FullName { get; set; }
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}