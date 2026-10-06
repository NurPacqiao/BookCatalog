namespace BookCatalog.Domain.Common;
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