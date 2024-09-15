using CSharpFunctionalExtensions;

namespace AlbinRonnkvist.HybridSearch.Api.Services;

public interface ISearcher<TResponse>
{
    public Task<Result<TResponse, string>> KeywordSearch(string query);
    // Add semantic search
    // Add hybrid search
    // Add all above combined
}
