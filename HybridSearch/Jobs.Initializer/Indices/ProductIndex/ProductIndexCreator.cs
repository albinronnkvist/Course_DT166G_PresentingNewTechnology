using AlbinRonnkvist.HybridSearch.Core.Constants;
using AlbinRonnkvist.HybridSearch.Jobs.Initializer.Services;
using CSharpFunctionalExtensions;

namespace AlbinRonnkvist.HybridSearch.Jobs.Initializer.Indices.ProductIndex;

public class ProductIndexCreator(IIndexManager indexManager) : IProductIndexCreator
{
    private readonly IIndexManager _indexManager = indexManager;

    public async Task<Result<int, string>> GenerateNextIndexVersion(CancellationToken ct)
    {
        return await _indexManager.GenerateNextIndexVersion(ProductIndexConstants.IndexName, ct);
    }

    public async Task<UnitResult<string>> CreateIndex(int version, CancellationToken ct)
    {
        return await _indexManager.CreateIndex(ProductIndexConstants.IndexName, version, ct);
    }

    public async Task<UnitResult<string>> EnsureHealthyIndex(int version, CancellationToken ct)
    {
        return await _indexManager.EnsureHealthyIndex(ProductIndexConstants.IndexName, version, ct);
    }

    public async Task<UnitResult<string>> ReassignSearchAlias(int version, CancellationToken ct)
    {
        return await _indexManager.ReassignSearchAlias(ProductIndexConstants.IndexName, version, ct);
    }
}
