using BookCatalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookCatalog.Infrastructure.Persistence;

public class BookCatalogDbContext(DbContextOptions<BookCatalogDbContext> options)
    : DbContext(options)
{
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Loan> Loans => Set<Loan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookCatalogDbContext).Assembly);
    }
}