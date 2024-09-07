using CSharpFunctionalExtensions;

namespace AlbinRonnkvist.HybridSearch.Embedding.Services;

public interface IEmbeddingGenerator
{
    Task<Result<decimal[], string>> GenerateEmbedding(string text, int dimensions);
}
