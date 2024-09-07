namespace AlbinRonnkvist.HybridSearch.Jobs.Initializer.Options;

public class ProductIndexOptions
{
    public required int Version { get; init; }

    public required bool AddSearchAlias { get; init; }
    public required string EmbeddingDimensions { get; init; }
}
