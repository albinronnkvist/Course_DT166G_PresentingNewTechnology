using AlbinRonnkvist.HybridSearch.Core.Helpers;
using CSharpFunctionalExtensions;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Cluster;
using Elastic.Clients.Elasticsearch.IndexManagement;

namespace AlbinRonnkvist.HybridSearch.Job.Initializer.Services;

public class IndexManager(ElasticsearchClient elasticsearchClient) : IIndexManager
{
    private readonly ElasticsearchClient _elasticsearchClient = elasticsearchClient;

    public async Task<Result<(int? OldVersion, int NewVersion), string>> GenerateNextIndexVersion(string indexName, CancellationToken ct)
    {
        var indexNamePattern = IndexNamingConvention.GetTemplatePattern(indexName);

        var response = await _elasticsearchClient.Indices.GetAsync(indexNamePattern, ct);
        if (!response.IsValidResponse)
        {
            return CSharpFunctionalExtensions.Result.Failure<(int? OldVersion, int NewVersion), string>($"Invalid response when fetching indices by search alias: {response.DebugInformation}");
        }

        if (response.Indices.Count is 0)
        {
            return CSharpFunctionalExtensions.Result.Success<(int? OldVersion, int NewVersion), string>((null, 1));
        }

        var currentIndexName = response.Indices.Keys.First().ToString();

        var versionResult = IndexNamingConvention.ExtractVersionFromIndexName(currentIndexName);
        if(versionResult.IsFailure)
        {
            return CSharpFunctionalExtensions.Result.Failure<(int? OldVersion, int NewVersion), string>(versionResult.Error);
        }

        return CSharpFunctionalExtensions.Result.Success<(int? OldVersion, int NewVersion), string>((versionResult.Value, versionResult.Value + 1));
    }
    
    public async Task<UnitResult<string>> CreateIndex(string indexName, int newVersion, CancellationToken ct)
    {
        var response = await _elasticsearchClient.Indices
            .CreateAsync(IndexNamingConvention.GetVersionedIndexName(indexName, newVersion), ct);

        if (!response.IsValidResponse)
        {
            return UnitResult.Failure(response.DebugInformation);
        }

        return UnitResult.Success<string>();
    }

    public async Task<UnitResult<string>> EnsureHealthyIndex(string indexName, int newVersion, CancellationToken ct)
    {
        var newVersionedIndexName = IndexNamingConvention.GetVersionedIndexName(indexName, newVersion);
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

    public async Task<UnitResult<string>> ReassignSearchAlias(string indexName, int? oldVersion, int newVersion, CancellationToken ct)
    {
        var searchAlias = IndexNamingConvention.GetSearchAlias(indexName);

        if(oldVersion is not null)
        {
            var oldVersionedIndexName = IndexNamingConvention.GetVersionedIndexName(indexName, oldVersion.Value);
            var removeAliasesResponse = await _elasticsearchClient.Indices
                .UpdateAliasesAsync(a => a
                    .Actions(actions => actions
                        .Remove(r => r.Index(oldVersionedIndexName).Alias(searchAlias))
                    ), ct);

            if (!removeAliasesResponse.IsValidResponse)
            {
                return UnitResult.Failure(removeAliasesResponse.DebugInformation);
            }
        }

        var newVersionedIndexName = IndexNamingConvention.GetVersionedIndexName(indexName, newVersion);
        var updateAliasesResponse = await _elasticsearchClient.Indices
            .UpdateAliasesAsync(a => a
                .Actions(actions => actions
                    .Add<AddAction>(a => a.Alias(searchAlias).Index(newVersionedIndexName))
                ), ct);

        if (!updateAliasesResponse.IsValidResponse)
        {
            return UnitResult.Failure(updateAliasesResponse.DebugInformation);
        }

        return UnitResult.Success<string>();
    }

    public async Task<UnitResult<string>> RemoveOldIndex(string indexName, int? oldVersion, CancellationToken ct)
    {
        if(oldVersion is null)
        {
            return UnitResult.Success<string>();
        }

        var oldVersionedIndexName = IndexNamingConvention.GetVersionedIndexName(indexName, oldVersion.Value);

        var deleteResponse = await _elasticsearchClient.Indices.DeleteAsync(oldVersionedIndexName, ct);
        if (!deleteResponse.IsValidResponse)
        {
            return UnitResult.Failure(deleteResponse.DebugInformation);
        }

        return UnitResult.Success<string>();
    }
}
