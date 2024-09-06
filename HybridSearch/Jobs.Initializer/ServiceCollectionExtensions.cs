using AlbinRonnkvist.HybridSearch.Jobs.Initializer.Options;
using AlbinRonnkvist.HybridSearch.Jobs.Initializer.Services.IndexTemplates;
using AlbinRonnkvist.HybridSearch.Jobs.Initializer.Indices.ProductIndex;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using AlbinRonnkvist.HybridSearch.Jobs.Initializer.Services;

namespace AlbinRonnkvist.HybridSearch.Jobs.Initializer;

public static class ServiceCollectionExtensions
{
    public static void ConfigureElasticsearch(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        var elasticsearchOptions = configuration.GetSection(nameof(ElasticsearchOptions))
            .Get<ElasticsearchOptions>() ?? throw new ArgumentNullException(nameof(ElasticsearchOptions), "Elasticsearch options are not configured properly");

        var elasticSearchSettings = CreateElasticsearchSettings(environment, elasticsearchOptions);

        services.AddSingleton(new ElasticsearchClient(elasticSearchSettings));
    }

    private static ElasticsearchClientSettings CreateElasticsearchSettings(IHostEnvironment environment, ElasticsearchOptions elasticsearchOptions)
    {
        if(environment.IsDevelopment())
        {
            return new ElasticsearchClientSettings(new Uri(elasticsearchOptions.Url))
                .DisableDirectStreaming();
        }

        return new ElasticsearchClientSettings(new Uri(elasticsearchOptions.Url))
                    .CertificateFingerprint(elasticsearchOptions.FingerPrint)
                    .Authentication(new BasicAuthentication(elasticsearchOptions.Username, elasticsearchOptions.Password));
    }

    public static void ConfigureCustomServices(this IServiceCollection services)
    {
        services.AddTransient<IIndexTemplateManager, IndexTemplateManager>();
        services.AddTransient<IIndexManager, IndexManager>();
    }

    public static void ConfigureIndices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ProductIndexOptions>(configuration.GetSection(nameof(ProductIndexOptions)));
        services.AddTransient<IProductIndexTemplateCreator, ProductIndexTemplateCreator>();
        services.AddTransient<IProductIndexCreator, ProductIndexCreator>();
    }
}
