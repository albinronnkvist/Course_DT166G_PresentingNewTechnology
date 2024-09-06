using CSharpFunctionalExtensions;

namespace AlbinRonnkvist.HybridSearch.Jobs.Initializer.Services.Indices.Product;

public interface IProductIndexTemplate
{
    Task<UnitResult<string>> CreateIndexTemplate();
}
