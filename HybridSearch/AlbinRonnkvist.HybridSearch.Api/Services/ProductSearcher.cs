using AlbinRonnkvist.HybridSearch.Api.Helpers.Mappers;
using AlbinRonnkvist.HybridSearch.Core.Constants;
using AlbinRonnkvist.HybridSearch.Core.Helpers;
using AlbinRonnkvist.HybridSearch.Api.Dtos;
using CSharpFunctionalExtensions;
using Elastic.Clients.Elasticsearch;

namespace AlbinRonnkvist.HybridSearch.Api.Services;

public class ProductSearcher(ElasticsearchClient elasticsearchClient) : ISearcher<ProductSearchResponse>
{
    private readonly ElasticsearchClient _elasticsearchClient = elasticsearchClient;

    public async Task<Result<ProductSearchResponse, string>> KeywordSearch(string query, int pageNumber, int pageSize)
    {
        var searchDescriptor = new SearchRequestDescriptor<Core.Models.Product>()
            .Index(IndexNamingConvention.GetSearchAlias(ProductIndexConstants.IndexName))
            .From(pageNumber * pageSize)
            .Size(pageSize)
            .Query(q => q
                .Match(m => m
                    .Field(f => f.Title)
                    .Query(query)
                )
            );

        var response = await _elasticsearchClient.SearchAsync(searchDescriptor);
        if (!response.IsValidResponse)
        {
            return CSharpFunctionalExtensions.Result.Failure<ProductSearchResponse, string>("Invalid response from Elasticsearch");
        }

        var products = ProductMapper.Map(response.Documents);
        var productSearchResponse = new ProductSearchResponse
        {
            Query = query,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPageHits = response.Hits.Count,
            TotalHits = response.Total,
            ServerResponseTime = response.Took,
            Products = products
        };

        return CSharpFunctionalExtensions.Result.Success<ProductSearchResponse, string>(productSearchResponse);
    }
}
