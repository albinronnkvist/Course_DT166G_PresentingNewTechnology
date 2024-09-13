namespace AlbinRonnkvist.HybridSearch.Job.Initializer.Options;

public class ProductIndexOptions
{
    public required int EmbeddingDimensions { get; init; }
    public required bool WaitForGreenHealth { get; init; }
}
