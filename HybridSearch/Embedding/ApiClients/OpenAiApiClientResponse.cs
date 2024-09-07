namespace AlbinRonnkvist.HybridSearch.Embedding.ApiClients;

public record OpenAiApiClientResponse
{
    public required decimal[] Embedding { get; init; }
}
