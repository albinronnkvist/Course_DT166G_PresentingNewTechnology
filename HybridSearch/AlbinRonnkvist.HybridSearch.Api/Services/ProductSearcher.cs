using AlbinRonnkvist.HybridSearch.Api.Helpers.Mappers;
using AlbinRonnkvist.HybridSearch.Core.Constants;
using AlbinRonnkvist.HybridSearch.Core.Helpers;
using AlbinRonnkvist.HybridSearch.Api.Dtos;
using CSharpFunctionalExtensions;
using Elastic.Clients.Elasticsearch;
using AlbinRonnkvist.HybridSearch.Embedding.Services;
using AlbinRonnkvist.HybridSearch.Api.Helpers;

namespace AlbinRonnkvist.HybridSearch.Api.Services;

public class ProductSearcher(ElasticsearchClient elasticsearchClient,
    IEmbeddingGenerator embeddingGenerator) : ISearcher<ProductSearchResponse>
{
    private readonly ElasticsearchClient _elasticsearchClient = elasticsearchClient;
    private readonly IEmbeddingGenerator _embeddingGenerator = embeddingGenerator;


    public async Task<Result<ProductSearchResponse, string>> KeywordSearch(string query, int pageNumber, int pageSize)
    {
        var response = await ExecuteKeywordSearch(query, pageNumber, pageSize);
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


    public async Task<Result<ProductSearchResponse, string>> SemanticSearch(string query, int pageNumber, int pageSize)
    {
        var response = await ExecuteSemanticSearch(query, pageNumber, pageSize);
        if (response.IsFailure || !response.Value.IsValidResponse)
        {
            return CSharpFunctionalExtensions.Result.Failure<ProductSearchResponse, string>("Invalid response from Elasticsearch");
        }

        var products = ProductMapper.Map(response.Value.Documents);
        var productSearchResponse = new ProductSearchResponse
        {
            Query = query,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPageHits = response.Value.Hits.Count,
            TotalHits = response.Value.Total,
            ServerResponseTime = response.Value.Took,
            Products = products
        };

        return CSharpFunctionalExtensions.Result.Success<ProductSearchResponse, string>(productSearchResponse);
    }

    public async Task<Result<ProductSearchResponse, string>> HybridSearch(string query, int pageNumber, int pageSize)
    {
        var keywordSearchTask = ExecuteKeywordSearch(query, pageNumber, pageSize);
        var semanticSearchTask = ExecuteSemanticSearch(query, pageNumber, pageSize);
        await Task.WhenAll(keywordSearchTask, semanticSearchTask);

        var keywordSearchResult = await keywordSearchTask;
        var semanticSearchResult = await semanticSearchTask;
        if(!keywordSearchResult.IsValidResponse || 
            semanticSearchResult.IsFailure ||
            !semanticSearchResult.Value.IsValidResponse)
        {
            return CSharpFunctionalExtensions.Result.Failure<ProductSearchResponse, string>("Invalid response from Elasticsearch");
        }

        var combinedResult = RRFCombiner.Combine(keywordSearchResult, semanticSearchResult.Value, pageSize);

        var products = ProductMapper.Map(combinedResult);
        var productSearchResponse = new ProductSearchResponse
        {
            Query = query,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPageHits = products.Count,
            TotalHits = keywordSearchResult.Total + semanticSearchResult.Value.Total, // Not correct but no other option right now
            ServerResponseTime = keywordSearchResult.Took > semanticSearchResult.Value.Took ? keywordSearchResult.Took : semanticSearchResult.Value.Took,
            Products = products
        };

        return CSharpFunctionalExtensions.Result.Success<ProductSearchResponse, string>(productSearchResponse);
    }


    private async Task<SearchResponse<Core.Models.Product>> ExecuteKeywordSearch(string query, int pageNumber, int pageSize)
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
        return response;
    }

    private async Task<Result<SearchResponse<Core.Models.Product>, string>> ExecuteSemanticSearch(string query, int pageNumber, int pageSize)
    {
        var embeddingResult = await _embeddingGenerator.GenerateEmbedding(query, ProductIndexConstants.EmbeddingDimensions);
        if (embeddingResult.IsFailure)
        {
            return CSharpFunctionalExtensions.Result.Failure<SearchResponse<Core.Models.Product>, string>(embeddingResult.Error);
        }

        var queryVector = embeddingResult.Value.Select(d => (float)d).ToArray();
        var searchDescriptor = new SearchRequestDescriptor<Core.Models.Product>()
            .Index(IndexNamingConvention.GetSearchAlias(ProductIndexConstants.IndexName))
            .From(pageNumber * pageSize)
            .Size(pageSize)
            .Query(q => q
                .Knn(m => m
                    .Field(f => f.TitleEmbedding)
                    .QueryVector(queryVector)
                    .k(10)
                    .NumCandidates(30)
                )
            );

        return await _elasticsearchClient.SearchAsync(searchDescriptor);
    }
}
