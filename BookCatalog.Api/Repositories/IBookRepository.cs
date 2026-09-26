using BookCatalog.Api.Models;

namespace BookCatalog.Api.Repositories;

public interface IBookRepository
{
    IEnumerable<Book> GetAll();
    Book? GetById(Guid id);
    void Add(Book book);
    bool Update(Book book);
    bool Delete(Guid id);
}