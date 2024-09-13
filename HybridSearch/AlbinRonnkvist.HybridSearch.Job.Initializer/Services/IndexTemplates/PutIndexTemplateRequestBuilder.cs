using Elastic.Clients.Elasticsearch.IndexManagement;
using AlbinRonnkvist.HybridSearch.Core.Helpers;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Mapping;

namespace AlbinRonnkvist.HybridSearch.Job.Initializer.Services.IndexTemplates;

public class PutIndexTemplateRequestBuilder
{
    private readonly PutIndexTemplateRequest _templateRequest;
    private readonly string _indexName;
    
    public PutIndexTemplateRequestBuilder(string indexName, int version)
    {
        _indexName = indexName;
        _templateRequest = new PutIndexTemplateRequest(IndexNamingConvention.GetTemplateName(indexName))
        {
            IndexPatterns = new[] { IndexNamingConvention.GetTemplatePattern(_indexName) },
            Template = new IndexTemplateMapping
            {
                Aliases = new Dictionary<IndexName, Alias>
                {
                    { IndexNamingConvention.GetVersionedAlias(_indexName, version), new Alias() }
                },
                Mappings = new TypeMapping
                {
                    Dynamic = DynamicMapping.True
                },
                Settings = new IndexSettings
                {
                    AutoExpandReplicas = "1-5",
                    NumberOfShards = 3,
                    NumberOfReplicas = 1
                }
            },
            Version = version
        };
    }

    public PutIndexTemplateRequestBuilder WithSearchAlias()
    {
        _templateRequest.Template?.Aliases?.Add(IndexNamingConvention.GetSearchAlias(_indexName), new Alias() );
        return this;
    }

    public PutIndexTemplateRequestBuilder WithCustomMappings(TypeMapping customMapping)
    {
        if (_templateRequest.Template is not null)
        {
            _templateRequest.Template.Mappings = customMapping;
        }
        return this;
    }
    
    public PutIndexTemplateRequest Build()
    {
        return _templateRequest;
    }
}
