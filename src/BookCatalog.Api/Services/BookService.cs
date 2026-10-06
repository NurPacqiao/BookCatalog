using BookCatalog.Api.DTOs;
using BookCatalog.Api.Mappings;
using BookCatalog.Domain.Common;      
using BookCatalog.Domain.Interfaces;

namespace BookCatalog.Api.Services;

public class BookService(IBookRepository repository, ILogger<BookService> logger) : IBookService
{
   
    public async Task<PagedResponse<BookResponse>> GetAllAsync(BookQueryParameters parameters)
{
    logger.LogInformation("Retrieving books: Page {Page}, Size {PageSize}", parameters.Page, parameters.PageSize);
    
    var (books, totalCount) = await repository.GetAllAsync(parameters);
    var bookResponses = books.Select(b => b.ToResponseDto());

    return new PagedResponse<BookResponse>(
        Items: bookResponses,
        TotalCount: totalCount,
        Page: parameters.Page,
        PageSize: parameters.PageSize
    );
}

    public async Task<BookResponse?> GetByIdAsync(Guid id)
    {
        var book = await repository.GetByIdAsync(id);
        return book?.ToResponseDto();
    }

    public async Task<BookResponse> CreateAsync(CreateBookRequest request)
    {
        var book = request.ToEntity();
        await repository.AddAsync(book);
        logger.LogInformation("Created book with ID: {BookId}", book.Id);
        return book.ToResponseDto();
    }

    public async Task<BookResponse?> UpdateAsync(Guid id, UpdateBookRequest request)
    {
        var existingBook = await repository.GetByIdAsync(id);
        if (existingBook is null) return null;

        existingBook.UpdateEntity(request);
        await repository.UpdateAsync(existingBook);
        logger.LogInformation("Updated book with ID: {BookId}", id);

        return existingBook.ToResponseDto();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await repository.DeleteAsync(id);
    }
}