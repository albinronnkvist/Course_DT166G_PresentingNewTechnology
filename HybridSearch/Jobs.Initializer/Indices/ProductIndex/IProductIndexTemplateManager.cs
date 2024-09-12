using CSharpFunctionalExtensions;

namespace AlbinRonnkvist.HybridSearch.Jobs.Initializer.Indices.ProductIndex;

public interface IProductIndexTemplateManager
{
    Task<UnitResult<string>> CreateIndexTemplate(int newVersion, bool addSearchAlias, CancellationToken ct);
}
