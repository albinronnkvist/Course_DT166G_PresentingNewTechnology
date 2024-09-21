namespace AlbinRonnkvist.HybridSearch.WebApp.Dtos;

public record ProductSearchRequest
{
    public required string Query { get; set; }
    public int? PageNumber { get; init; }
    public int? PageSize { get; init; }
}
