namespace AlbinRonnkvist.HybridSearch.Embedding.ApiClients;

using CSharpFunctionalExtensions;

public interface IEmbeddingApiClient<TRequest, TResponse>
{
    Task<Result<TResponse, string>> GetEmbedding(TRequest request);
}
