using AlbinRonnkvist.HybridSearch.Core.Helpers;
using CSharpFunctionalExtensions;
using Elastic.Clients.Elasticsearch;

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
}
