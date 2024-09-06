using CSharpFunctionalExtensions;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.IndexManagement;

namespace AlbinRonnkvist.HybridSearch.Jobs.Initializer.Services.IndexTemplates;

public class IndexTemplateManager : IIndexTemplateManager
{
    private readonly ElasticsearchClient _elasticsearchClient;

    public IndexTemplateManager(ElasticsearchClient elasticsearchClient)
    {
        _elasticsearchClient = elasticsearchClient;
    }
    
    public async Task<UnitResult<string>> UpsertIndexTemplate(PutIndexTemplateRequest request)
    {
        var response = await _elasticsearchClient.Indices.PutIndexTemplateAsync(request);
        if (!response.IsValidResponse)
        {
            return UnitResult.Failure(response.DebugInformation);
        }

        return UnitResult.Success<string>();
    }
}
