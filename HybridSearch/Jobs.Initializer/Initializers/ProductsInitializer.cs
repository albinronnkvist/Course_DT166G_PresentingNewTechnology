using AlbinRonnkvist.HybridSearch.Embedding.Services;
using AlbinRonnkvist.HybridSearch.Jobs.Initializer.Indices.ProductIndex;
using AlbinRonnkvist.HybridSearch.Jobs.Initializer.Initializers;
using AlbinRonnkvist.HybridSearch.Jobs.Initializer.Options;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;

namespace AlbinRonnkvist.HybridSearch.Jobs.Initializer;

public class ProductsInitializer(ILogger<ProductsInitializer> logger,
    IOptions<ProductIndexOptions> options,
    IProductIndexTemplateCreator productIndexTemplateCreator,
    IProductIndexCreator productIndexCreator,
    IEmbeddingGenerator embeddingGenerator) : IInitializer
{
    private readonly ILogger<ProductsInitializer> _logger = logger;
    private readonly ProductIndexOptions _options = options.Value;
    private readonly IProductIndexTemplateCreator _productIndexTemplateCreator = productIndexTemplateCreator;
    private readonly IProductIndexCreator _productIndexCreator = productIndexCreator;
    private readonly IEmbeddingGenerator _embeddingGenerator = embeddingGenerator;

    public async Task<Result<string, string>> Execute(CancellationToken ct)
    {        
        _logger.LogInformation("Starting initialization of products index...");
        
        _logger.LogInformation("Generating new index version...");
        var nextIndexVersionResult = await _productIndexCreator.GenerateNextIndexVersion(ct);
        if(nextIndexVersionResult.IsFailure)
        {
            return Result.Failure<string, string>(nextIndexVersionResult.Error);
        }

        _logger.LogInformation("Upserting index template...");
        var preIndexTemplateResult = await _productIndexTemplateCreator.CreateIndexTemplate(nextIndexVersionResult.Value, false, ct);
        if(preIndexTemplateResult.IsFailure)
        {
            return Result.Failure<string, string>(preIndexTemplateResult.Error);
        }

        _logger.LogInformation("Creating new index...");
        var indexCreationResult = await _productIndexCreator.CreateIndex(nextIndexVersionResult.Value,ct);
        if(indexCreationResult.IsFailure) 
        {
            return Result.Failure<string, string>(indexCreationResult.Error);
        }

        _logger.LogInformation("Populating indices (TODO)...");
        //
        // TODO: Populate new indices with documents
        // 

        if(_options.WaitForGreenHealth)
        {
            _logger.LogInformation("Ensuring healthy index (times out after 60 seconds if not healthy)...");

            var ensureHealthyIndexResult = await _productIndexCreator.EnsureHealthyIndex(nextIndexVersionResult.Value, ct);
            if(ensureHealthyIndexResult.IsFailure)
            {
                return Result.Failure<string, string>(ensureHealthyIndexResult.Error);
            } 
        }

        _logger.LogInformation("Reassigning search alias to new index...");
        var reassignSearchAlias = await _productIndexCreator.ReassignSearchAlias(nextIndexVersionResult.Value, ct);
        if(reassignSearchAlias.IsFailure)
        {
            return Result.Failure<string, string>(reassignSearchAlias.Error);
        }

        _logger.LogInformation("Deleting old indices (TODO)...");
        // Delete old indices (previous versions)

        _logger.LogInformation("Upserting index template...");
        var postIndexTemplateResult = await _productIndexTemplateCreator.CreateIndexTemplate(nextIndexVersionResult.Value, true, ct);
        if(postIndexTemplateResult.IsFailure)
        {
            return Result.Failure<string, string>(postIndexTemplateResult.Error);
        }

        _logger.LogInformation("Done!");
        return Result.Success<string, string>("Success");






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
