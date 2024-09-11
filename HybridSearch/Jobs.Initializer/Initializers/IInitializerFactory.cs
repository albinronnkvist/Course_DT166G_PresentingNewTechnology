namespace AlbinRonnkvist.HybridSearch.Jobs.Initializer.Initializers;

public interface IInitializerFactory
{
    IEnumerable<IInitializer> CreateInitializers();
}
