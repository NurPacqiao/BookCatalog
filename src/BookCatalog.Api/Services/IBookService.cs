using BookCatalog.Api.DTOs;
using BookCatalog.Domain.Common;

namespace BookCatalog.Api.Services;

public interface IBookService
{
    Task<PagedResponse<BookResponse>> GetAllAsync(BookQueryParameters parameters);
    Task<BookResponse?> GetByIdAsync(Guid id);
    Task<BookResponse> CreateAsync(CreateBookRequest request);
    Task<BookResponse?> UpdateAsync(Guid id, UpdateBookRequest request);
    Task<bool> DeleteAsync(Guid id);
}