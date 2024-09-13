using CSharpFunctionalExtensions;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.IndexManagement;

namespace AlbinRonnkvist.HybridSearch.Job.Initializer.Services.IndexTemplates;

public class IndexTemplateManager(ElasticsearchClient elasticsearchClient) : IIndexTemplateManager
{
    private readonly ElasticsearchClient _elasticsearchClient = elasticsearchClient;

    public async Task<UnitResult<string>> UpsertIndexTemplate(PutIndexTemplateRequest request, CancellationToken ct)
    {
        var response = await _elasticsearchClient.Indices.PutIndexTemplateAsync(request, ct);
        if (!response.IsValidResponse)
        {
            return UnitResult.Failure(response.DebugInformation);
        }

        return UnitResult.Success<string>();
    }
}
