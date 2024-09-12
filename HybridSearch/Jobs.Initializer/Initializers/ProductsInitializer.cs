using AlbinRonnkvist.HybridSearch.Embedding.Services;
using AlbinRonnkvist.HybridSearch.Jobs.Initializer.Indices.ProductIndex;
using AlbinRonnkvist.HybridSearch.Jobs.Initializer.Initializers;
using AlbinRonnkvist.HybridSearch.Jobs.Initializer.Options;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;

namespace AlbinRonnkvist.HybridSearch.Jobs.Initializer;

public class ProductsInitializer(ILogger<ProductsInitializer> logger,
    IProductIndexTemplateCreator productIndexTemplateCreator,
    IProductIndexCreator productIndexCreator,
    IEmbeddingGenerator embeddingGenerator) : IInitializer
{
    private readonly ILogger<ProductsInitializer> _logger = logger;
    private readonly IProductIndexTemplateCreator _productIndexTemplateCreator = productIndexTemplateCreator;
    private readonly IProductIndexCreator _productIndexCreator = productIndexCreator;
    private readonly IEmbeddingGenerator _embeddingGenerator = embeddingGenerator;

    public async Task<Result<string, string>> Execute(CancellationToken ct)
    {        
        _logger.LogInformation("Starting initialization of products index...");
        
        var nextIndexVersionResult = await _productIndexCreator.GenerateNextIndexVersion(ct);
        if(nextIndexVersionResult.IsFailure)
        {
            return Result.Failure<string, string>(nextIndexVersionResult.Error);
        }

        var preIndexTemplateResult = await _productIndexTemplateCreator.CreateIndexTemplate(nextIndexVersionResult.Value, false, ct);
        if(preIndexTemplateResult.IsFailure)
        {
            return Result.Failure<string, string>(preIndexTemplateResult.Error);
        }

        var indexCreationResult = await _productIndexCreator.CreateIndex(nextIndexVersionResult.Value,ct);
        if(indexCreationResult.IsFailure) 
        {
            return Result.Failure<string, string>(indexCreationResult.Error);
        }

        //
        // TODO: Populate new indices with documents
        // 

        var ensureHealthyIndexResult = await _productIndexCreator.EnsureHealthyIndex(nextIndexVersionResult.Value, ct);
        if(ensureHealthyIndexResult.IsFailure)
        {
            return Result.Failure<string, string>(ensureHealthyIndexResult.Error);
        }

        var reassignSearchAlias = await _productIndexCreator.ReassignSearchAlias(nextIndexVersionResult.Value, ct);
        if(reassignSearchAlias.IsFailure)
        {
            return Result.Failure<string, string>(reassignSearchAlias.Error);
        }

        // Delete old indices (previous versions)

        // Update template so new product indices added later are searchable (basically a reset)
        var postIndexTemplateResult = await _productIndexTemplateCreator.CreateIndexTemplate(nextIndexVersionResult.Value, true, ct);
        if(postIndexTemplateResult.IsFailure)
        {
            return Result.Failure<string, string>(postIndexTemplateResult.Error);
        }

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
