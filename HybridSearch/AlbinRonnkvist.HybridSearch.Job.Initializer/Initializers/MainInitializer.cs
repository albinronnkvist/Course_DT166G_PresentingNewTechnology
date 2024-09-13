namespace AlbinRonnkvist.HybridSearch.Job.Initializer.Initializers;

public class MainInitializer(IInitializerFactory initializerFactory, ILogger<MainInitializer> logger)
{
    private readonly IInitializerFactory _initializerFactory = initializerFactory;
    private readonly ILogger<MainInitializer> _logger = logger;

    public async Task ExecuteAllInitializersAsync(CancellationToken ct)
    {
        try
        {
            var initializers = _initializerFactory.CreateInitializers();
            foreach (var initializer in initializers)
            {
                var result = await initializer.Execute(ct);
                if (result.IsSuccess)
                {
                    _logger.LogInformation("Success: {SuccessResult}", result.Value);
                    continue;
                }
                
                if (result.IsFailure)
                {
                    throw new Exception(result.Error);
                }
            }

            _logger.LogInformation("All initializers executed successfully");
        }
        catch(Exception ex) {
            _logger.LogError(ex, "Failed to execute initializers: {ErrorResult}", ex.Message);
            throw;
        }
    }
}
