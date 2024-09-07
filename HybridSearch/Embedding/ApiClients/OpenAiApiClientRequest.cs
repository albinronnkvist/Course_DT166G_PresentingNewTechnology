namespace AlbinRonnkvist.HybridSearch.Embedding.ApiClients;

public record OpenAiApiClientRequest
{
    public required string Input { get; init; }

    public required string Model { get; init; }
}
