namespace BookCatalog.Api.DTOs;
public record CreateBookRequest(
    string Title,
    Guid AuthorId,
    string Isbn,
    int PublicationYear,
    decimal Price);

public record UpdateBookRequest(
    string Title,
    Guid AuthorId,
    string Isbn,
    int PublicationYear,
    decimal Price);
public record BookResponse(
    Guid Id,
    string Title,
    Guid AuthorId,
    string AuthorName,
    string Isbn,
    int PublicationYear,
    decimal Price);

public record PagedResponse<T>(
    IEnumerable<T> Items,
    int TotalCount,
    int Page,
    int PageSize
)
{
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;
}