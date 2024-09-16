namespace AlbinRonnkvist.HybridSearch.Job.Initializer.Options;

public class ProductIndexOptions
{
    public required bool GenerateEmbeddings { get; init; }
    public required bool WaitForGreenHealth { get; init; }
}
