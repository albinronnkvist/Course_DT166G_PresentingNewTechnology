using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AlbinRonnkvist.HybridSearch.Jobs.Initializer;

public class ProductsInitializer : BackgroundService
{
    private readonly ILogger<ProductsInitializer> _logger;

    public ProductsInitializer(ILogger<ProductsInitializer> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("ProductsInitializer running at: {time}", DateTimeOffset.Now);
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}
