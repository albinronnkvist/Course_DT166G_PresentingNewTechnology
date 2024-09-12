using CSharpFunctionalExtensions;

namespace AlbinRonnkvist.HybridSearch.Jobs.Initializer.Indices.ProductIndex;

public interface IProductIndexCreator
{
    Task<Result<int, string>> GenerateNextIndexVersion(CancellationToken ct);
    Task<UnitResult<string>> CreateIndex(CancellationToken ct);
}
