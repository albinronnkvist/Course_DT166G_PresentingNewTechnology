using AlbinRonnkvist.HybridSearch.Core.Constants;
using AlbinRonnkvist.HybridSearch.Job.Initializer.Services;
using CSharpFunctionalExtensions;

namespace AlbinRonnkvist.HybridSearch.Job.Initializer.Indices.ProductIndex;

public class ProductIndexManager(IIndexManager indexManager) : IProductIndexManager
{
    private readonly IIndexManager _indexManager = indexManager;

    public async Task<Result<(int? OldVersion, int NewVersion), string>> GenerateNextIndexVersion(CancellationToken ct)
    {
        return await _indexManager.GenerateNextIndexVersion(ProductIndexConstants.IndexName, ct);
    }

    public async Task<UnitResult<string>> CreateIndex(int newVersion, CancellationToken ct)
    {
        return await _indexManager.CreateIndex(ProductIndexConstants.IndexName, newVersion, ct);
    }

    public async Task<UnitResult<string>> EnsureHealthyIndex(int newVersion, CancellationToken ct)
    {
        return await _indexManager.EnsureHealthyIndex(ProductIndexConstants.IndexName, newVersion, ct);
    }

    public async Task<UnitResult<string>> ReassignSearchAlias(int? oldVersion, int newVersion, CancellationToken ct)
    {
        return await _indexManager.ReassignSearchAlias(ProductIndexConstants.IndexName, oldVersion, newVersion, ct);
    }

    public async Task<UnitResult<string>> RemoveOldIndex(int? oldVersion, CancellationToken ct)
    {
        return await _indexManager.RemoveOldIndex(ProductIndexConstants.IndexName, oldVersion, ct);
    }
}
