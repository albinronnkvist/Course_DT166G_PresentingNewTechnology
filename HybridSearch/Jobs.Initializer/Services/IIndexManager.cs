using CSharpFunctionalExtensions;

namespace AlbinRonnkvist.HybridSearch.Jobs.Initializer.Services;

public interface IIndexManager
{
    Task<UnitResult<string>> CreateIndex(string indexName, int version, CancellationToken ct);
}
