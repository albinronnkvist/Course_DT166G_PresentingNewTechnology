namespace AlbinRonnkvist.HybridSearch.Job.Initializer.Indices.ProductIndex;

using CSharpFunctionalExtensions;

public interface IProductIndexManager
{
    Task<Result<(int? OldVersion, int NewVersion), string>> GenerateNextIndexVersion(CancellationToken ct);
    Task<UnitResult<string>> CreateIndex(int newVersion, CancellationToken ct);
    Task<UnitResult<string>> EnsureHealthyIndex(int newVersion, CancellationToken ct);
    Task<UnitResult<string>> ReassignSearchAlias(int? oldVersion, int newVersion, CancellationToken ct);
    Task<UnitResult<string>> RemoveOldIndex(int? oldVersion, CancellationToken ct);
}
