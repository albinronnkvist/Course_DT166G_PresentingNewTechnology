namespace AlbinRonnkvist.HybridSearch.Embedding.Options;

public record OpenAiEmbeddingApiOptions
{
    public required string BaseUrl { get; init; }
    public required string Model { get; init; }
    public required string AccessToken { get; init; }
    public required string OrganizationId { get; init; }
    public required string ProjectId { get; init; }
}
