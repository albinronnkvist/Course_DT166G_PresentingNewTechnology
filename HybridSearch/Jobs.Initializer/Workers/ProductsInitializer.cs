using AlbinRonnkvist.HybridSearch.Embedding.Services;
using AlbinRonnkvist.HybridSearch.Jobs.Initializer.Indices.ProductIndex;
using AlbinRonnkvist.HybridSearch.Jobs.Initializer.Options;
using Microsoft.Extensions.Options;

namespace AlbinRonnkvist.HybridSearch.Jobs.Initializer;

public class ProductsInitializer(ILogger<ProductsInitializer> logger,
    IOptions<ProductIndexOptions> options,
    IProductIndexTemplateCreator productIndexTemplateCreator,
    IProductIndexCreator productIndexCreator,
    IEmbeddingGenerator embeddingGenerator) : BackgroundService
{
    private readonly ILogger<ProductsInitializer> _logger = logger;
    private readonly ProductIndexOptions _options = options.Value;
    private readonly IProductIndexTemplateCreator _productIndexTemplateCreator = productIndexTemplateCreator;
    private readonly IProductIndexCreator _productIndexCreator = productIndexCreator;
    private readonly IEmbeddingGenerator _embeddingGenerator = embeddingGenerator;

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        // TODO: move logic to separate services
        
        // Uncomment to create index templates and indices
        /*
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
        */

        // Uncomment to generate an embedding
        /*
        var restult = await _embeddingGenerator.GenerateEmbedding("test", _options.EmbeddingDimensions);
        if(restult.IsSuccess)
        {
            _logger.LogInformation("Embedding generated: {Embedding}", restult.Value);
        }
        else
        {
            _logger.LogError("Failed to generate embedding: {Error}", restult.Error);
        } */
    }
}
