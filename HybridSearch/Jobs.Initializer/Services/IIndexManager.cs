using CSharpFunctionalExtensions;

namespace AlbinRonnkvist.HybridSearch.Jobs.Initializer.Services;

public interface IIndexManager
{
    Task<Result<int, string>> GenerateNextIndexVersion(string indexName, CancellationToken ct);
    Task<UnitResult<string>> CreateIndex(string indexName, int version, CancellationToken ct);
    Task<UnitResult<string>> EnsureHealthyIndex(string indexName, int version, CancellationToken ct);
    Task<UnitResult<string>> ReassignSearchAlias(string indexName, int version, CancellationToken ct);
}
