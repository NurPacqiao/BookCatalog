using BookCatalog.Api.DTOs;
using BookCatalog.Domain.Entities;

namespace BookCatalog.Api.Mappings;

public static class BookMappingExtensions
{
    public static Book ToEntity(this CreateBookRequest dto) => new()
{
    Id = Guid.NewGuid(),
    Title = dto.Title,
    Isbn = dto.Isbn,
    PublicationYear = dto.PublicationYear,
    Price = dto.Price,
    AuthorId = dto.AuthorId
};

public static void UpdateEntity(this Book book, UpdateBookRequest dto)
{
    book.Title = dto.Title;
    book.Isbn = dto.Isbn;
    book.PublicationYear = dto.PublicationYear;
    book.Price = dto.Price;
    book.AuthorId = dto.AuthorId;
}

public static BookResponse ToResponseDto(this Book book) => new(
    Id: book.Id,
    Title: book.Title,
    AuthorId: book.AuthorId,
    AuthorName: book.Author?.Name ?? string.Empty,
    Isbn: book.Isbn,
    PublicationYear: book.PublicationYear,
    Price: book.Price);
}