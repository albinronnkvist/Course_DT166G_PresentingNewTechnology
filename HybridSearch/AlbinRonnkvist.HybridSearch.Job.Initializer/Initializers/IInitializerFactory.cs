namespace AlbinRonnkvist.HybridSearch.Job.Initializer.Initializers;

public interface IInitializerFactory
{
    IEnumerable<IInitializer> CreateInitializers();
}
