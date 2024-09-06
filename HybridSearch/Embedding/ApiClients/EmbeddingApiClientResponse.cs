namespace AlbinRonnkvist.HybridSearch.Embedding.ApiClients;

public record EmbeddingApiClientResponse
{
    public required decimal[] Embedding { get; init; }
}
