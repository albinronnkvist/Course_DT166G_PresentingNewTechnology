namespace AlbinRonnkvist.HybridSearch.Embedding.ApiClients;

public record EmbeddingApiClientRequest
{
    public required string Text { get; init; }
}
