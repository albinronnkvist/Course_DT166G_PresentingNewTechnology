namespace AlbinRonnkvist.HybridSearch.WebApp.Dtos;

public record ProductSearchResponse
{
    public required string Query { get; init; }
    public required int PageNumber { get; init; }
    public required int PageSize { get; init; }
    public required int TotalPageHits { get; init; }
    public required long TotalHits { get; init; }
    public required long ServerResponseTime { get; init; }
    public required List<Product> Products { get; init; }
}
