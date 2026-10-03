using BookCatalog.Api.DTOs;
using BookCatalog.Api.Models;

namespace BookCatalog.Api.Repositories;

public interface IBookRepository
{
    Task<(IEnumerable<Book> Items, int TotalCount)> GetAllAsync(BookQueryParameters parameters);
    Task<Book?> GetByIdAsync(Guid id);
    Task AddAsync(Book book);
    Task<bool> UpdateAsync(Book book);
    Task<bool> DeleteAsync(Guid id);
}