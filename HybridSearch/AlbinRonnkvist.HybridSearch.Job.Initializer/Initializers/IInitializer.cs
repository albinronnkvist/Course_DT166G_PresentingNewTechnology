using CSharpFunctionalExtensions;

namespace AlbinRonnkvist.HybridSearch.Job.Initializer.Initializers;

public interface IInitializer
{
    Task<Result<string, string>> Execute(CancellationToken ct);
}
