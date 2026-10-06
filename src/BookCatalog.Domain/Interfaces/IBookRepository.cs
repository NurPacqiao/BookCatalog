using BookCatalog.Domain.Entities;
using BookCatalog.Domain.Common;

namespace BookCatalog.Domain.Interfaces;

public interface IBookRepository
{
    Task<(IEnumerable<Book> Items, int TotalCount)> GetAllAsync(BookQueryParameters parameters);
    Task<Book?> GetByIdAsync(Guid id);
    Task AddAsync(Book book);
    Task<bool> UpdateAsync(Book book);
    Task<bool> DeleteAsync(Guid id);
}