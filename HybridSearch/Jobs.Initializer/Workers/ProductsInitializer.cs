using AlbinRonnkvist.HybridSearch.Jobs.Initializer.Indices.ProductIndex;
using Elastic.Clients.Elasticsearch;

namespace AlbinRonnkvist.HybridSearch.Jobs.Initializer;

public class ProductsInitializer(ILogger<ProductsInitializer> logger,
    ElasticsearchClient elasticsearchClient,
    IProductIndexTemplate productIndexTemplate) : BackgroundService
{
    private readonly ILogger<ProductsInitializer> _logger = logger;
    private readonly ElasticsearchClient _elasticsearchClient = elasticsearchClient;
    private readonly IProductIndexTemplate _productIndexTemplate = productIndexTemplate;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var res = await _productIndexTemplate.CreateIndexTemplate();
        if(res.IsSuccess)
        {
            _logger.LogInformation("Index 'products' created");
        }

        else {
            _logger.LogError("Failed to create index 'products': {ay}", res.Error);
        }

        await Task.Delay(2000, stoppingToken);

        _logger.LogInformation("ProductsInitializer running at: {time}", DateTimeOffset.Now);
        var response = await _elasticsearchClient.Indices.ExistsAsync("products", stoppingToken);
        _logger.LogInformation("Index 'products' exists: {exists} {ay}", response.Exists, response.DebugInformation);
    }
}
