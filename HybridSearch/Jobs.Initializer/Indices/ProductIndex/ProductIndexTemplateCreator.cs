using AlbinRonnkvist.HybridSearch.Core.Constants;
using AlbinRonnkvist.HybridSearch.Jobs.Initializer.Options;
using AlbinRonnkvist.HybridSearch.Jobs.Initializer.Services.IndexTemplates;
using CSharpFunctionalExtensions;
using Elastic.Clients.Elasticsearch.Mapping;
using Microsoft.Extensions.Options;

namespace AlbinRonnkvist.HybridSearch.Jobs.Initializer.Indices.ProductIndex;

public class ProductIndexTemplateCreator : IProductIndexTemplateCreator
{
    private readonly IIndexTemplateManager _indexTemplateManager;
    private readonly ProductIndexOptions _options;

    public ProductIndexTemplateCreator(IOptions<ProductIndexOptions> options, IIndexTemplateManager indexTemplateManager)
    {
        _options = options.Value;
        _indexTemplateManager = indexTemplateManager;
    }

    public async Task<UnitResult<string>> CreateIndexTemplate(CancellationToken ct)
    {
        var request = new PutIndexTemplateRequestBuilder(ProductIndexConstants.IndexName, _options.Version)
            .WithCustomMappings(new TypeMapping
            {
                Properties = GetProperties(_options.EmbeddingDimensions)
            });

        if (_options.AddSearchAlias)
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
