using BookCatalog.Api.Models;

namespace BookCatalog.Api.Repositories;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(Guid id);
    Task AddAsync(Book book);
    Task<bool> UpdateAsync(Book book);
    Task<bool> DeleteAsync(Guid id);
}