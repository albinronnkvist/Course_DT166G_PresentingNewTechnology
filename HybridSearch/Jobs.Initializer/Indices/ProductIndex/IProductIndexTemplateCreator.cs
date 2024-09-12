using CSharpFunctionalExtensions;

namespace AlbinRonnkvist.HybridSearch.Jobs.Initializer.Indices.ProductIndex;

public interface IProductIndexTemplateCreator
{
    Task<UnitResult<string>> CreateIndexTemplate(int version, bool addSearchAlias, CancellationToken ct);
}
