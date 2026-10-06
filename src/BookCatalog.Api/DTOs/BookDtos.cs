namespace BookCatalog.Api.DTOs;

public record CreateBookRequest(
    string Title,
    string Author,
    string Isbn,
    int PublicationYear,
    decimal Price
);

public record UpdateBookRequest(
    string Title,
    string Author,
    string Isbn,
    int PublicationYear,
    decimal Price
);

public record BookResponse(
    Guid Id,
    string Title,
    string Author,
    string Isbn,
    int PublicationYear,
    decimal Price
);

public record BookQueryParameters(
    string? Search = null,
    int? Year = null,
    int Page = 1,
    int PageSize = 10
)
{
    public int Page { get; init; } = Page < 1 ? 1 : Page;
    public int PageSize { get; init; } = PageSize switch
    {
        < 1 => 10,
        > 50 => 50,
        _ => PageSize
    };
}

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