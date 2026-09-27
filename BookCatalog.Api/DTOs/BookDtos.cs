namespace BookCatalog.Api.DTOs;

// What the client sends when creating a book
public record CreateBookRequest(
    string Title,
    string Author,
    string Isbn,
    int PublicationYear,
    decimal Price
);

// What the client sends when updating a book
public record UpdateBookRequest(
    string Title,
    string Author,
    string Isbn,
    int PublicationYear,
    decimal Price
);

// What we return back to the client
public record BookResponse(
    Guid Id,
    string Title,
    string Author,
    string Isbn,
    int PublicationYear,
    decimal Price
);