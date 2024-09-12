using AlbinRonnkvist.HybridSearch.Core.Constants;
using AlbinRonnkvist.HybridSearch.Jobs.Initializer.Options;
using AlbinRonnkvist.HybridSearch.Jobs.Initializer.Services;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;

namespace AlbinRonnkvist.HybridSearch.Jobs.Initializer.Indices.ProductIndex;

public class ProductIndexCreator(IOptions<ProductIndexOptions> options,
    IIndexManager indexManager) : IProductIndexCreator
{
    private readonly ProductIndexOptions _options = options.Value;
    private readonly IIndexManager _indexManager = indexManager;

    public async Task<Result<int, string>> GenerateNextIndexVersion(CancellationToken ct)
    {
        return await _indexManager.GenerateNextIndexVersion(ProductIndexConstants.IndexName, ct);
    }

    public async Task<UnitResult<string>> CreateIndex(CancellationToken ct)
    {
        return await _indexManager.CreateIndex(ProductIndexConstants.IndexName, _options.Version, ct);
    }

}
