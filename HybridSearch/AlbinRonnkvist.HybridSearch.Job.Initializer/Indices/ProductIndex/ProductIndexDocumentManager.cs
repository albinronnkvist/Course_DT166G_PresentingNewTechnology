using System.Collections.ObjectModel;
using System.Text.Json;
using AlbinRonnkvist.HybridSearch.Core.Constants;
using AlbinRonnkvist.HybridSearch.Core.Helpers;
using AlbinRonnkvist.HybridSearch.Core.Models;
using AlbinRonnkvist.HybridSearch.Embedding.Services;
using AlbinRonnkvist.HybridSearch.Job.Initializer.Options;
using AlbinRonnkvist.HybridSearch.Job.Initializer.Services;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;

namespace AlbinRonnkvist.HybridSearch.Job.Initializer.Indices.ProductIndex;

public class ProductIndexDocumentManager(IDocumentManager<Product> documentManager,
    IEmbeddingGenerator embeddingGenerator,
    IOptions<ProductIndexOptions> options) : IProductIndexDocumentManager
{
    private readonly IDocumentManager<Product> _documentManager = documentManager;
    private readonly IEmbeddingGenerator _embeddingGenerator = embeddingGenerator;
    private readonly ProductIndexOptions _options = options.Value;

    public async Task<UnitResult<string>> CreateDocuments(int newVersion, CancellationToken ct)
    {
        var newVersionedIndexName = IndexNamingConvention.GetVersionedIndexName(ProductIndexConstants.IndexName, newVersion);

        var extractDataResult = await ExtractData();
        if (extractDataResult.IsFailure)
        {
            return UnitResult.Failure<string>(extractDataResult.Error);
        }

        var transformDataResult = await TransformData(extractDataResult.Value);
        if (transformDataResult.IsFailure)
        {
            return UnitResult.Failure<string>(transformDataResult.Error);
        }

        var result = await _documentManager.CreateDocuments(newVersionedIndexName, transformDataResult.Value, ct);
        if (result.IsFailure)
        {
            return UnitResult.Failure<string>(result.Error);
        }

        return UnitResult.Success<string>();
    }

    // Temp solution
    private static async Task<Result<string, string>> ExtractData()
    {
        var baseDirectory = AppContext.BaseDirectory;
        var filePath = Path.Combine(baseDirectory, "Indices", "ProductIndex", "products-example.json");        
        if (!File.Exists(filePath))
        {
            return Result.Failure<string, string>("File not found: " + filePath);
        }

        var jsonData = await File.ReadAllTextAsync(filePath);

        return Result.Success<string, string>(jsonData);
    }

    private async Task<Result<ReadOnlyCollection<Product>, string>> TransformData(string extractedData)
    {
        if(string.IsNullOrWhiteSpace(extractedData))
        {
            return Result.Failure<ReadOnlyCollection<Product>, string>("No data provided");
        }

        var productDtos = JsonSerializer.Deserialize<List<ProductDto>>(extractedData);
        if (productDtos is null || productDtos.Count is 0) 
        {
            return Result.Failure<ReadOnlyCollection<Product>, string>("Failed to deserialize products");
        }

        var products = new List<Product>();
        foreach (var product in productDtos)
        {
            var embedding = await _embeddingGenerator.GenerateEmbedding(product.Title, _options.EmbeddingDimensions);
            if (embedding.IsFailure)
            {
                return Result.Failure<ReadOnlyCollection<Product>, string>("Failed to generate embedding for product: " + product.Title);
            }

            products.Add(new Product
            {
                Id = product.Id,
                Title = product.Title,
                TitleEmbedding = embedding.Value
            });
        }

        return Result.Success<ReadOnlyCollection<Product>, string>(products.AsReadOnly());
    }
}
