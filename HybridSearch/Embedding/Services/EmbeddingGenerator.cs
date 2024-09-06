using AlbinRonnkvist.HybridSearch.Embedding.ApiClients;
using CSharpFunctionalExtensions;

namespace AlbinRonnkvist.HybridSearch.Embedding.Services;

public class EmbeddingGenerator : IEmbeddingGenerator
{
    private readonly IEmbeddingApiClient _embeddingApiClient;

    public EmbeddingGenerator(IEmbeddingApiClient embeddingApiClient)
    {
        _embeddingApiClient = embeddingApiClient; 
    }

    public async Task<Result<decimal[], string>> GenerateEmbedding(string text)
    {
        var result = await _embeddingApiClient.GetEmbedding(text);
        if (result.IsFailure)
        {
            return Result.Failure<decimal[], string>(result.Error);
        }

        return Result.Success<decimal[], string>(result.Value.Embedding);
    }
}
