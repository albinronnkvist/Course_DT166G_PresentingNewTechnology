using AlbinRonnkvist.HybridSearch.Core.Helpers;
using CSharpFunctionalExtensions;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Cluster;
using Elastic.Clients.Elasticsearch.IndexManagement;

namespace AlbinRonnkvist.HybridSearch.Jobs.Initializer.Services;

public class IndexManager(ElasticsearchClient elasticsearchClient) : IIndexManager
{
    private readonly ElasticsearchClient _elasticsearchClient = elasticsearchClient;

    public async Task<Result<int, string>> GenerateNextIndexVersion(string indexName, CancellationToken ct)
    {
        var indexNamePattern = IndexNamingConvention.GetTemplatePattern(indexName);

        var response = await _elasticsearchClient.Indices.GetAsync(indexNamePattern, ct);
        if (!response.IsValidResponse)
        {
            return CSharpFunctionalExtensions.Result.Failure<int, string>($"Invalid response when fetching indices by search alias: {response.DebugInformation}");
        }

        if (response.Indices.Count is 0)
        {
            return CSharpFunctionalExtensions.Result.Success<int, string>(1);
        }

        var currentIndexName = response.Indices.Keys.First().ToString();

        var versionResult = IndexNamingConvention.ExtractVersionFromIndexName(currentIndexName);
        if(versionResult.IsFailure)
        {
            return CSharpFunctionalExtensions.Result.Failure<int, string>(versionResult.Error);
        }

        return CSharpFunctionalExtensions.Result.Success<int, string>(versionResult.Value + 1);
    }
    
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

    public async Task<UnitResult<string>> EnsureHealthyIndex(string indexName, int version, CancellationToken ct)
    {
        var newVersionedIndexName = IndexNamingConvention.GetVersionedIndexName(indexName, version);
        var healthStatusResponse = await _elasticsearchClient.Cluster.HealthAsync(new HealthRequest(newVersionedIndexName)
        {
            WaitForStatus = HealthStatus.Green,
            Timeout = "60s",
        }, ct);

        if (!healthStatusResponse.IsValidResponse)
        {
            return UnitResult.Failure("Failed to wait for green status: " + healthStatusResponse.DebugInformation);
        }

        if(healthStatusResponse.Status != HealthStatus.Green)
        {
            return UnitResult.Failure("Index is not green: " + healthStatusResponse.Status.ToString());
        }

        return UnitResult.Success<string>();
    }

    public async Task<UnitResult<string>> ReassignSearchAlias(string indexName, int version, CancellationToken ct)
    {
        var indexNamePattern = IndexNamingConvention.GetTemplatePattern(indexName);
        var newVersionedIndexName = IndexNamingConvention.GetVersionedIndexName(indexName, version);
        var searchAlias = IndexNamingConvention.GetSearchAlias(indexName);

        var updateAliasesResponse = await _elasticsearchClient.Indices
            .UpdateAliasesAsync(a => a
                .Actions(actions => actions
                    .Remove(r => r.Alias(searchAlias).Index(indexNamePattern))
                    .Add<AddAction>(a => a.Alias(searchAlias).Index(newVersionedIndexName))
                ), ct);

        if (!updateAliasesResponse.IsValidResponse)
        {
            return UnitResult.Failure(updateAliasesResponse.DebugInformation);
        }

        return UnitResult.Success<string>();
    }
}
