namespace AlbinRonnkvist.HybridSearch.Job.Initializer.Indices.ProductIndex;

public record ProductDto
{
    public required int Id { get; init; }
    public required string Title { get; init; } = default!;
}
