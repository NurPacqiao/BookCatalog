using BookCatalog.Domain.Common;
using BookCatalog.Domain.Entities;
using BookCatalog.Domain.Interfaces;
using BookCatalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookCatalog.Infrastructure.Repositories;

public class EfBookRepository(BookCatalogDbContext db) : IBookRepository
{
    public async Task<(IEnumerable<Book> Items, int TotalCount)> GetAllAsync(BookQueryParameters parameters)
    {
        IQueryable<Book> query = db.Books.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(parameters.Search))
        {
            var pattern = $"%{parameters.Search}%";
            query = query.Where(b =>
                EF.Functions.Like(b.Title, pattern) ||
                EF.Functions.Like(b.Author.Name, pattern));
        }

        if (parameters.Year.HasValue)
        {
            query = query.Where(b => b.PublicationYear == parameters.Year.Value);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(b => b.Title)
            .ThenBy(b => b.Id) // stable tie-breaker for consistent pagination
            .Skip((parameters.Page - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Book?> GetByIdAsync(Guid id) =>
        await db.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);

    public async Task AddAsync(Book book)
    {
        db.Books.Add(book);
        await db.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(Book book)
    {
        var exists = await db.Books.AnyAsync(b => b.Id == book.Id);
        if (!exists)
            return false;

        db.Books.Update(book);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var book = await db.Books.FirstOrDefaultAsync(b => b.Id == id);
        if (book is null)
            return false;

        db.Books.Remove(book);
        await db.SaveChangesAsync();
        return true;
    }
}