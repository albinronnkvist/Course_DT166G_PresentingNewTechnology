using System.Text.Json.Serialization;

namespace AlbinRonnkvist.HybridSearch.Embedding.ApiClients;

public record OpenAiApiClientResponse
{
    public required string Object { get; init; }

    public required List<OpenAiApiClientResponseData> Data { get; init; }

    public required string Model { get; init; }

    public required OpenAiApiClientResponseUsage Usage { get; init; }
}

public record OpenAiApiClientResponseData
{
    public required string Object { get; init; }

    public required int Index { get; init; }

    public required decimal[] Embedding { get; init; }
} 

public record OpenAiApiClientResponseUsage {
    [JsonPropertyName("prompt_tokens")]
    public required int PromptTokens { get; init; }

    [JsonPropertyName("total_tokens")]
    public required int TotalTokens { get; init; }
}
