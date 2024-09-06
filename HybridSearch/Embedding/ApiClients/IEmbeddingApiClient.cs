namespace AlbinRonnkvist.HybridSearch.Embedding.ApiClients;

using CSharpFunctionalExtensions;

public interface IEmbeddingApiClient
{
    Task<Result<EmbeddingApiClientResponse, string>> GetEmbedding(string text);
}
