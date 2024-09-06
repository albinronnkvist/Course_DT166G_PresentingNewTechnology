using CSharpFunctionalExtensions;

namespace AlbinRonnkvist.HybridSearch.Jobs.Initializer.Indices.ProductIndex;

public interface IProductIndexTemplate
{
    Task<UnitResult<string>> CreateIndexTemplate();
}
