namespace AlbinRonnkvist.HybridSearch.Api.Dtos;

public record SearchHit<TDocument>
{
    public required int Id { get; init; }
    public required TDocument Document { get; init; }
}
