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
    IEmbeddingGenerator embeddingGenerator, ILogger<ProductSearcher> logger) : ISearcher<ProductSearchResponse>
{
    private readonly ElasticsearchClient _elasticsearchClient = elasticsearchClient;
    private readonly IEmbeddingGenerator _embeddingGenerator = embeddingGenerator;
    private readonly ILogger<ProductSearcher> _logger = logger;

    public async Task<Result<ProductSearchResponse, string>> KeywordSearch(string query, int pageNumber, int pageSize)
    {
        var result = await ExecuteKeywordSearch(query, pageNumber, pageSize);
        if (result.IsFailure)
        {
            return CSharpFunctionalExtensions.Result.Failure<ProductSearchResponse, string>(result.Error);
        }

        var products = ProductMapper.Map(result.Value.Hits);
        var productSearchResponse = new ProductSearchResponse
        {
            Query = query,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPageHits = result.Value.Hits.Count,
            TotalHits = result.Value.TotalHits,
            ServerResponseTime = result.Value.ServerResponseTime,
            Products = products
        };

        return CSharpFunctionalExtensions.Result.Success<ProductSearchResponse, string>(productSearchResponse);
    }


    public async Task<Result<ProductSearchResponse, string>> SemanticSearch(string query, int pageNumber, int pageSize)
    {
        var result = await ExecuteSemanticSearch(query, pageNumber, pageSize);
        if (result.IsFailure)
        {
            return CSharpFunctionalExtensions.Result.Failure<ProductSearchResponse, string>("Invalid response from Elasticsearch");
        }

        var products = ProductMapper.Map(result.Value.Hits);
        var productSearchResponse = new ProductSearchResponse
        {
            Query = query,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPageHits = result.Value.Hits.Count,
            TotalHits = result.Value.TotalHits,
            ServerResponseTime = result.Value.ServerResponseTime,
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
        if(keywordSearchResult.IsFailure || semanticSearchResult.IsFailure)
        {
            return CSharpFunctionalExtensions.Result.Failure<ProductSearchResponse, string>("Invalid response from Elasticsearch");
        }

        var combinedResult = RRFCombiner.Combine(keywordSearchResult.Value, semanticSearchResult.Value, pageSize);

        var products = ProductMapper.Map(combinedResult);
        var productSearchResponse = new ProductSearchResponse
        {
            Query = query,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPageHits = products.Count,
            TotalHits = keywordSearchResult.Value.TotalHits + semanticSearchResult.Value.TotalHits, // Not correct but no other option right now
            ServerResponseTime = keywordSearchResult.Value.ServerResponseTime > semanticSearchResult.Value.ServerResponseTime 
                ? keywordSearchResult.Value.ServerResponseTime
                : semanticSearchResult.Value.ServerResponseTime,
            Products = products
        };

        return CSharpFunctionalExtensions.Result.Success<ProductSearchResponse, string>(productSearchResponse);
    }



    private async Task<Result<Dtos.SearchResponse<Core.Models.Product>, string>> ExecuteKeywordSearch(string query, int pageNumber, int pageSize)
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
        if(!response.IsValidResponse || response.TimedOut)
        {
            _logger.LogError("Failed to execute keyword search: {Error}", response.DebugInformation ?? "Unknown error");
            return CSharpFunctionalExtensions.Result.Failure<Dtos.SearchResponse<Core.Models.Product>, string>("Invalid response from Elasticsearch");
        }

        var mappedResponse = SearchResponseMapper.Map(response.Documents, response.Total, response.Took);
        return CSharpFunctionalExtensions.Result.Success<Dtos.SearchResponse<Core.Models.Product>, string>(mappedResponse);
    }

    private async Task<Result<Dtos.SearchResponse<Core.Models.Product>, string>> ExecuteSemanticSearch(string query, int pageNumber, int pageSize)
    {
        var embeddingResult = await _embeddingGenerator.GenerateEmbedding(query, ProductIndexConstants.EmbeddingDimensions);
        if (embeddingResult.IsFailure)
        {
            _logger.LogError("Failed to generate embeddings: {Error}", embeddingResult.Error);
            return CSharpFunctionalExtensions.Result.Failure<Dtos.SearchResponse<Core.Models.Product>, string>("Failed to generate embedding for query: " + query);
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
        
        var response = await _elasticsearchClient.SearchAsync(searchDescriptor);
        if(!response.IsValidResponse || response.TimedOut)
        {
            _logger.LogError("Failed to execute semantic search: {Error}", response.DebugInformation ?? "Unknown error");
            return CSharpFunctionalExtensions.Result.Failure<Dtos.SearchResponse<Core.Models.Product>, string>("Invalid response from Elasticsearch");
        }

        var mappedResponse = SearchResponseMapper.Map(response.Documents, response.Total, response.Took);
        return CSharpFunctionalExtensions.Result.Success<Dtos.SearchResponse<Core.Models.Product>, string>(mappedResponse);
    }
}
