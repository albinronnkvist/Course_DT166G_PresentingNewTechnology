namespace AlbinRonnkvist.HybridSearch.Embedding.Options;

public record EmbeddingApiOptions
{
    public required string BaseUrl { get; init; }
    public required string ModelId { get; init; }
    public required string AccessToken { get; init; }
}
