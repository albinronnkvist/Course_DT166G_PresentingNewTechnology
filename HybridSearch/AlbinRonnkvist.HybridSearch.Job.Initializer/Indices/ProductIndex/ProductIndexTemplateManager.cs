using AlbinRonnkvist.HybridSearch.Core.Constants;
using AlbinRonnkvist.HybridSearch.Job.Initializer.Services.IndexTemplates;
using CSharpFunctionalExtensions;
using Elastic.Clients.Elasticsearch.Mapping;

namespace AlbinRonnkvist.HybridSearch.Job.Initializer.Indices.ProductIndex;

public class ProductIndexTemplateManager(IIndexTemplateManager indexTemplateManager) : IProductIndexTemplateManager
{
    private readonly IIndexTemplateManager _indexTemplateManager = indexTemplateManager;

    public async Task<UnitResult<string>> CreateIndexTemplate(int newVersion, bool addSearchAlias, CancellationToken ct)
    {
        var request = new PutIndexTemplateRequestBuilder(ProductIndexConstants.IndexName, newVersion)
            .WithCustomMappings(new TypeMapping
            {
                Properties = GetProperties(ProductIndexConstants.EmbeddingDimensions)
            });

        if (addSearchAlias)
        {
            request.WithSearchAlias();
        }

        return await _indexTemplateManager.UpsertIndexTemplate(request.Build(), ct);
    }

    private static Properties GetProperties(int dimensions) {
        var properties = new Properties();
        properties.Add<Core.Models.Product>(x => x.Id, new IntegerNumberProperty());
        properties.Add<Core.Models.Product>(p => p.Title, new TextProperty());
        properties.Add<Core.Models.Product>(p => p.TitleEmbedding, new DenseVectorProperty { Dims = dimensions });

        return properties;
    }
}
