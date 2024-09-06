using AlbinRonnkvist.HybridSearch.Jobs.Initializer.Indices.ProductIndex;
using Elastic.Clients.Elasticsearch;

namespace AlbinRonnkvist.HybridSearch.Jobs.Initializer;

public class ProductsInitializer(ILogger<ProductsInitializer> logger,
    IProductIndexTemplateCreator productIndexTemplateCreator,
    IProductIndexCreator productIndexCreator) : BackgroundService
{
    private readonly ILogger<ProductsInitializer> _logger = logger;
    private readonly IProductIndexTemplateCreator _productIndexTemplateCreator = productIndexTemplateCreator;
    private readonly IProductIndexCreator _productIndexCreator = productIndexCreator;

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        var indexTemplateResult = await _productIndexTemplateCreator.CreateIndexTemplate(ct);
        var indexCreationResult = await _productIndexCreator.CreateIndex(ct);

        if(indexTemplateResult.IsSuccess)
        {
            _logger.LogInformation("Index template 'products-template' created");
        }
        else {
            _logger.LogError("Failed to create index template 'products-template': {Error}", indexTemplateResult.Error);
        }

        if(indexCreationResult.IsSuccess)
        {
            _logger.LogInformation("Index 'products' created");
        }
        else {
            _logger.LogError("Failed to create index 'products': {Error}", indexCreationResult.Error);
        }

        await StopAsync(ct);
    }
}
