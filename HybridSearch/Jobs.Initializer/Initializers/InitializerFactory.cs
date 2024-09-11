namespace AlbinRonnkvist.HybridSearch.Jobs.Initializer.Initializers;

public class InitializerFactory : IInitializerFactory
{
    private readonly IServiceProvider _serviceProvider;

    public InitializerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IEnumerable<IInitializer> CreateInitializers()
    {
        return _serviceProvider.GetServices<IInitializer>();
    }
}
