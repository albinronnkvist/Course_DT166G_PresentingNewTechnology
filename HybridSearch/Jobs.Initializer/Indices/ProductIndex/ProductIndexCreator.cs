using AlbinRonnkvist.HybridSearch.Core.Constants;
using AlbinRonnkvist.HybridSearch.Jobs.Initializer.Options;
using AlbinRonnkvist.HybridSearch.Jobs.Initializer.Services;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;

namespace AlbinRonnkvist.HybridSearch.Jobs.Initializer.Indices.ProductIndex;

public class ProductIndexCreator : IProductIndexCreator
{
    private readonly ProductIndexOptions _options;
    private readonly IIndexManager _indexManager;

    public ProductIndexCreator(IOptions<ProductIndexOptions> options, IIndexManager indexManager)
    {
        _options = options.Value;
        _indexManager = indexManager;
    }

    public async Task<UnitResult<string>> CreateIndex(CancellationToken ct)
    {
        return await _indexManager.CreateIndex(ProductIndexConstants.IndexName, _options.Version, ct);
    }
}
