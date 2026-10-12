using BookCatalog.Api.DTOs;
using BookCatalog.Domain.Entities;

namespace BookCatalog.Api.Mappings;

public static class BookMappingExtensions
{
    public static BookResponse ToResponseDto(this Book book) =>
        new(book.Id, book.Title, book.Author.Name, book.Isbn, book.PublicationYear, book.Price);

    public static Book ToEntity(this CreateBookRequest request) =>
        new()
        {
            Title = request.Title.Trim(),
            Author = new Author { Name = request.Author.Trim() },
            Isbn = request.Isbn.Trim(),
            PublicationYear = request.PublicationYear,
            Price = request.Price
        };

    public static void UpdateEntity(this Book book, UpdateBookRequest request)
    {
        book.Title = request.Title.Trim();
        book.Author.Name = request.Author.Trim();
        book.Isbn = request.Isbn.Trim();
        book.PublicationYear = request.PublicationYear;
        book.Price = request.Price;
    }
}