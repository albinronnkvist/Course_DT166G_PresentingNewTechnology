using CSharpFunctionalExtensions;

namespace AlbinRonnkvist.HybridSearch.Jobs.Initializer.Initializers;

public interface IInitializer
{
    Task<Result<string, string>> Execute(CancellationToken ct);
}
