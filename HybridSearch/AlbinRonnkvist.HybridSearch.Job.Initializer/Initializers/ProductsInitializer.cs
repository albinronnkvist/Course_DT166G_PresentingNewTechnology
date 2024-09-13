using AlbinRonnkvist.HybridSearch.Job.Initializer.Indices.ProductIndex;
using AlbinRonnkvist.HybridSearch.Job.Initializer.Initializers;
using AlbinRonnkvist.HybridSearch.Job.Initializer.Options;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;

namespace AlbinRonnkvist.HybridSearch.Job.Initializer;

public class ProductsInitializer(ILogger<ProductsInitializer> logger,
    IOptions<ProductIndexOptions> options,
    IProductIndexTemplateManager productIndexTemplateManager,
    IProductIndexManager productIndexManager,
    IProductIndexDocumentManager productIndexDocumentManager) : IInitializer
{
    private readonly ILogger<ProductsInitializer> _logger = logger;
    private readonly ProductIndexOptions _options = options.Value;
    private readonly IProductIndexTemplateManager _productIndexTemplateManager = productIndexTemplateManager;
    private readonly IProductIndexManager _productIndexManager = productIndexManager;
    private readonly IProductIndexDocumentManager _productIndexDocumentManager = productIndexDocumentManager;

    public async Task<Result<string, string>> Execute(CancellationToken ct)
    {        
        _logger.LogInformation("Starting initialization of products index...");
        
        _logger.LogInformation("Generating new index version...");
        var nextIndexVersionResult = await _productIndexManager.GenerateNextIndexVersion(ct);
        if(nextIndexVersionResult.IsFailure)
        {
            return Result.Failure<string, string>(nextIndexVersionResult.Error);
        }

        var (OldVersion, NewVersion) = nextIndexVersionResult.Value;

        _logger.LogInformation("Upserting index template...");
        var preIndexTemplateResult = await _productIndexTemplateManager.CreateIndexTemplate(NewVersion, false, ct);
        if(preIndexTemplateResult.IsFailure)
        {
            return Result.Failure<string, string>(preIndexTemplateResult.Error);
        }

        _logger.LogInformation("Creating new index...");
        var indexCreationResult = await _productIndexManager.CreateIndex(NewVersion,ct);
        if(indexCreationResult.IsFailure) 
        {
            return Result.Failure<string, string>(indexCreationResult.Error);
        }

        _logger.LogInformation("Populating indices...");
        var createDocumentsResult = await _productIndexDocumentManager.CreateDocuments(NewVersion, ct);
        if(createDocumentsResult.IsFailure)
        {
            return Result.Failure<string, string>(createDocumentsResult.Error);
        }

        if(_options.WaitForGreenHealth)
        {
            _logger.LogInformation("Ensuring healthy index (times out after 60 seconds if not healthy)...");

            var ensureHealthyIndexResult = await _productIndexManager.EnsureHealthyIndex(NewVersion, ct);
            if(ensureHealthyIndexResult.IsFailure)
            {
                return Result.Failure<string, string>(ensureHealthyIndexResult.Error);
            } 
        }

        _logger.LogInformation("Reassigning search alias to new index...");
        var reassignSearchAlias = await _productIndexManager.ReassignSearchAlias(OldVersion, NewVersion, ct);
        if(reassignSearchAlias.IsFailure)
        {
            return Result.Failure<string, string>(reassignSearchAlias.Error);
        }

        _logger.LogInformation("Deleting old indices...");
        var removeResult = await _productIndexManager.RemoveOldIndex(OldVersion, ct);
        if(removeResult.IsFailure)
        {
            return Result.Failure<string, string>(removeResult.Error);
        }

        _logger.LogInformation("Upserting index template...");
        var postIndexTemplateResult = await _productIndexTemplateManager.CreateIndexTemplate(NewVersion, true, ct);
        if(postIndexTemplateResult.IsFailure)
        {
            return Result.Failure<string, string>(postIndexTemplateResult.Error);
        }

        return Result.Success<string, string>("Product index initialized successfully");
    }
}
