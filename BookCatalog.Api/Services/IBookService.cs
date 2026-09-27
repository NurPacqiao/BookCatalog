// Services/IBookService.cs
using BookCatalog.Api.DTOs;

namespace BookCatalog.Api.Services;

public interface IBookService
{
    Task<IEnumerable<BookResponse>> GetAllAsync();
    Task<BookResponse?> GetByIdAsync(Guid id);
    Task<BookResponse> CreateAsync(CreateBookRequest request);
    Task<BookResponse?> UpdateAsync(Guid id, UpdateBookRequest request);
    Task<bool> DeleteAsync(Guid id);
}