using CSharpFunctionalExtensions;

namespace AlbinRonnkvist.HybridSearch.Job.Initializer.Indices.ProductIndex;

public interface IProductIndexTemplateManager
{
    Task<UnitResult<string>> CreateIndexTemplate(int newVersion, bool addSearchAlias, CancellationToken ct);
}
