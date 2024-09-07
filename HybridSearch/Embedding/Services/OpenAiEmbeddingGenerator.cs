using AlbinRonnkvist.HybridSearch.Embedding.ApiClients;
using AlbinRonnkvist.HybridSearch.Embedding.Options;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;

namespace AlbinRonnkvist.HybridSearch.Embedding.Services;

public class OpenAiEmbeddingGenerator(IOptions<OpenAiEmbeddingApiOptions> options,
    IEmbeddingApiClient<OpenAiApiClientRequest, OpenAiApiClientResponse> embeddingApiClient) : IEmbeddingGenerator
{
    private readonly OpenAiEmbeddingApiOptions _options = options.Value;
    private readonly IEmbeddingApiClient<OpenAiApiClientRequest, OpenAiApiClientResponse> _embeddingApiClient = embeddingApiClient;

    public async Task<Result<decimal[], string>> GenerateEmbedding(string text, int dimensions)
    {
        var request = new OpenAiApiClientRequest
        {
            Input = text,
            Model = _options.Model,
            Dimensions = dimensions
        };

        var result = await _embeddingApiClient.GetEmbedding(request);
        if (result.IsFailure)
        {
            return Result.Failure<decimal[], string>(result.Error);
        }

        return Result.Success<decimal[], string>(result.Value.Data.Embedding);
    }
}
