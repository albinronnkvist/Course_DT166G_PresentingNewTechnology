using Elastic.Clients.Elasticsearch;

namespace AlbinRonnkvist.HybridSearch.Jobs.Initializer;

public class ProductsInitializer(ILogger<ProductsInitializer> logger, ElasticsearchClient elasticsearchClient) : BackgroundService
{
    private readonly ILogger<ProductsInitializer> _logger = logger;
    private readonly ElasticsearchClient _elasticsearchClient = elasticsearchClient;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("ProductsInitializer running at: {time}", DateTimeOffset.Now);
                var response = await _elasticsearchClient.Indices.ExistsAsync("products", stoppingToken);
                _logger.LogInformation("Index 'products' exists: {exists} {ay}", response.Exists, response.DebugInformation);
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}
