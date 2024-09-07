namespace AlbinRonnkvist.HybridSearch.Embedding.Options;

public record OpenAiEmbeddingApiOptions
{
    public required string BaseUrl { get; init; }
    public required string Model { get; init; }
    public required string AccessToken { get; init; }
}
