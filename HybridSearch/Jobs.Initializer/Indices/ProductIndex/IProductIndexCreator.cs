using CSharpFunctionalExtensions;

namespace AlbinRonnkvist.HybridSearch.Jobs.Initializer.Indices.ProductIndex;

public interface IProductIndexCreator
{
    Task<Result<(int? OldVersion, int NewVersion), string>> GenerateNextIndexVersion(CancellationToken ct);
    Task<UnitResult<string>> CreateIndex(int version, CancellationToken ct);
    Task<UnitResult<string>> EnsureHealthyIndex(int version, CancellationToken ct);
    Task<UnitResult<string>> ReassignSearchAlias(int? oldVersion, int newVersion, CancellationToken ct);
    Task<UnitResult<string>> RemoveOldIndex(int? oldVersion, CancellationToken ct);
}
