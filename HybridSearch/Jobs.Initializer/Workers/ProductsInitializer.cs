using AlbinRonnkvist.HybridSearch.Embedding.Services;
using AlbinRonnkvist.HybridSearch.Jobs.Initializer.Indices.ProductIndex;

namespace AlbinRonnkvist.HybridSearch.Jobs.Initializer;

public class ProductsInitializer(ILogger<ProductsInitializer> logger,
    IProductIndexTemplateCreator productIndexTemplateCreator,
    IProductIndexCreator productIndexCreator,
    IEmbeddingGenerator embeddingGenerator) : BackgroundService
{
    private readonly ILogger<ProductsInitializer> _logger = logger;
    private readonly IProductIndexTemplateCreator _productIndexTemplateCreator = productIndexTemplateCreator;
    private readonly IProductIndexCreator _productIndexCreator = productIndexCreator;
    private readonly IEmbeddingGenerator _embeddingGenerator = embeddingGenerator;

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        var restult = await _embeddingGenerator.GenerateEmbedding("test", 768);
        if(restult.IsSuccess)
        {
            _logger.LogInformation("Embedding generated: {Embedding}", restult.Value);
        }
        else
        {
            _logger.LogError("Failed to generate embedding: {Error}", restult.Error);
        }
/*         var indexTemplateResult = await _productIndexTemplateCreator.CreateIndexTemplate(ct);
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
 */
    }
}
