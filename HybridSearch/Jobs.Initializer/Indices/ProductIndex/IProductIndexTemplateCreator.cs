using CSharpFunctionalExtensions;

namespace AlbinRonnkvist.HybridSearch.Jobs.Initializer.Indices.ProductIndex;

public interface IProductIndexTemplateCreator
{
    Task<UnitResult<string>> CreateIndexTemplate(CancellationToken ct);
}
