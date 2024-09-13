namespace AlbinRonnkvist.HybridSearch.Job.Initializer.Initializers;

public class InitializerFactory(IServiceProvider serviceProvider) : IInitializerFactory
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public IEnumerable<IInitializer> CreateInitializers()
    {
        return _serviceProvider.GetServices<IInitializer>();
    }
}
