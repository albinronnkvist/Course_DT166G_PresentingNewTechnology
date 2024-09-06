using AlbinRonnkvist.HybridSearch.Core.Helpers;
using CSharpFunctionalExtensions;
using Elastic.Clients.Elasticsearch;

namespace AlbinRonnkvist.HybridSearch.Jobs.Initializer.Services;

public class IndexManager(ElasticsearchClient elasticsearchClient) : IIndexManager
{
    private readonly ElasticsearchClient _elasticsearchClient = elasticsearchClient;
    
    public async Task<UnitResult<string>> CreateIndex(string indexName, int version, CancellationToken ct)
    {
        var response = await _elasticsearchClient.Indices
            .CreateAsync(IndexNamingConvention.GetVersionedIndexName(indexName, version), ct);

        if (!response.IsValidResponse)
        {
            return UnitResult.Failure(response.DebugInformation);
        }

        return UnitResult.Success<string>();
    }
}
