namespace AlbinRonnkvist.HybridSearch.Api.Dtos;

public record ProductSearchSanitizedRequest
{
    public required string Query { get; set; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}
