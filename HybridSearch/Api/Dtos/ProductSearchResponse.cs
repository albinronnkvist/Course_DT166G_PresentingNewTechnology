namespace AlbinRonnkvist.HybridSearch.Api.Dtos;

public record ProductSearchResponse
{
    public string? Query { get; init; }
    public int Hits { get; init; }
    public required List<Product> Products { get; init; }
}
