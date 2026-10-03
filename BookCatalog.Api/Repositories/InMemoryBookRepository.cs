using System.Collections.Concurrent;
using BookCatalog.Api.DTOs;
using BookCatalog.Api.Models;

namespace BookCatalog.Api.Repositories;

public class InMemoryBookRepository : IBookRepository
{
    private static readonly ConcurrentDictionary<Guid, Book> _books = new();

    public Task<(IEnumerable<Book> Items, int TotalCount)> GetAllAsync(BookQueryParameters parameters)
    {
        IEnumerable<Book> query = _books.Values;

        // 1. Search in Title OR Author
        if (!string.IsNullOrWhiteSpace(parameters.Search))
        {
            query = query.Where(b => 
                b.Title.Contains(parameters.Search, StringComparison.OrdinalIgnoreCase) ||
                b.Author.Contains(parameters.Search, StringComparison.OrdinalIgnoreCase));
        }

        // 2. Filter by Year if provided
        if (parameters.Year.HasValue)
        {
            query = query.Where(b => b.PublicationYear == parameters.Year.Value);
        }

        // 3. Count total matching items before slicing
        var totalCount = query.Count();

        // 4. Slice the current page
        var items = query
            .Skip((parameters.Page - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToList();

        return Task.FromResult((items.AsEnumerable(), totalCount));
    }

    public Task<Book?> GetByIdAsync(Guid id)
    {
        _books.TryGetValue(id, out var book);
        return Task.FromResult(book);
    }

    public Task AddAsync(Book book)
    {
        _books[book.Id] = book;
        return Task.CompletedTask;
    }

    public Task<bool> UpdateAsync(Book book)
    {
        if (!_books.ContainsKey(book.Id))
            return Task.FromResult(false);

        _books[book.Id] = book;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(Guid id) =>
        Task.FromResult(_books.TryRemove(id, out _));
}