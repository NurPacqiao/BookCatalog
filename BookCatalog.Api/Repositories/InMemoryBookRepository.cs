using System.Collections.Concurrent;
using BookCatalog.Api.Models;

namespace BookCatalog.Api.Repositories;

public class InMemoryBookRepository : IBookRepository
{
    private readonly ConcurrentDictionary<Guid, Book> _books = new();

    public Task<IEnumerable<Book>> GetAllAsync() =>
        Task.FromResult<IEnumerable<Book>>(_books.Values);

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