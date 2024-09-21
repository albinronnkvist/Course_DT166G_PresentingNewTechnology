using AlbinRonnkvist.HybridSearch.WebApp.Dtos;
using CSharpFunctionalExtensions;

namespace AlbinRonnkvist.HybridSearch.WebApp.ApiClients;

public interface IHybridSearchApiClient
{
    Task<Result<ProductSearchResponse, string>> ProductSearch(ProductSearchRequest request);
}
