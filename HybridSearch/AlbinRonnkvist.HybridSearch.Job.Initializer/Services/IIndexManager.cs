using CSharpFunctionalExtensions;

namespace AlbinRonnkvist.HybridSearch.Job.Initializer.Services;

public interface IIndexManager
{
    Task<Result<(int? OldVersion, int NewVersion), string>> GenerateNextIndexVersion(string indexName, CancellationToken ct);
    Task<UnitResult<string>> CreateIndex(string indexName, int newVersion, CancellationToken ct);
    Task<UnitResult<string>> EnsureHealthyIndex(string indexName, int newVersion, CancellationToken ct);
    Task<UnitResult<string>> ReassignSearchAlias(string indexName, int? oldVersion, int newVersion, CancellationToken ct);
    Task<UnitResult<string>> RemoveOldIndex(string indexName, int? oldVersion, CancellationToken ct);
}
