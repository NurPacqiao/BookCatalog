using System.Collections.Concurrent;
using BookCatalog.Api.Models;

namespace BookCatalog.Api.Repositories;

public class InMemoryBookRepository : IBookRepository
{
    // A thread-safe dictionary: Key = Guid (Book ID), Value = Book object
    private readonly ConcurrentDictionary<Guid, Book> _books = new();

    public IEnumerable<Book> GetAll()
    {
        return _books.Values;
    }

    public Book? GetById(Guid id)
    {
        _books.TryGetValue(id, out var book);
        return book;
    }

    public void Add(Book book)
    {
        _books[book.Id] = book;
    }

    public bool Update(Book book)
    {
        if (!_books.ContainsKey(book.Id))
        {
            return false;
        }

        _books[book.Id] = book;
        return true;
    }

    public bool Delete(Guid id)
    {
        return _books.TryRemove(id, out _);
    }
}