namespace AlbinRonnkvist.HybridSearch.Embedding.Options;

public record EmbeddingApiOptions
{
    public required string Url { get; init; }
    public required string AccessToken { get; init; }
}
