using AlbinRonnkvist.HybridSearch.Job.Initializer.Options;
using AlbinRonnkvist.HybridSearch.Job.Initializer.Services.IndexTemplates;
using AlbinRonnkvist.HybridSearch.Job.Initializer.Indices.ProductIndex;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using AlbinRonnkvist.HybridSearch.Job.Initializer.Services;
using AlbinRonnkvist.HybridSearch.Core.Models;

namespace AlbinRonnkvist.HybridSearch.Job.Initializer;

internal static class ServiceCollectionExtensions
{
    internal static void ConfigureJobsInitializerProject(this IServiceCollection services,
        IConfiguration configuration, IHostEnvironment environment) 
    {
        services.ConfigureElasticsearch(configuration, environment);
        services.ConfigureIndices(configuration);
        services.ConfigureCustomServices();
    }

    private static void ConfigureElasticsearch(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.Configure<ElasticsearchOptions>(configuration.GetSection(nameof(ElasticsearchOptions)));
        services.AddOptionsWithValidateOnStart<ElasticsearchOptions, ElasticsearchOptionsValidator>(nameof(ElasticsearchOptions));

        var elasticsearchOptions = configuration.
            GetSection(nameof(ElasticsearchOptions))
            .Get<ElasticsearchOptions>() ?? throw new InvalidOperationException("ElasticsearchOptions is not configured properly.");
        
        var elasticSearchSettings = ConfigureElasticsearchSettings(environment, elasticsearchOptions);

        services.AddSingleton(new ElasticsearchClient(elasticSearchSettings));
    }

    private static ElasticsearchClientSettings ConfigureElasticsearchSettings(IHostEnvironment environment, ElasticsearchOptions elasticsearchOptions)
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

    private static void ConfigureIndices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ProductIndexOptions>(configuration.GetSection(nameof(ProductIndexOptions)));
        services.AddOptionsWithValidateOnStart<ProductIndexOptions, ProductIndexOptionsValidator>(nameof(ProductIndexOptions));

        services.AddTransient<IProductIndexTemplateManager, ProductIndexTemplateManager>();
        services.AddTransient<IProductIndexManager, ProductIndexManager>();
        services.AddScoped<IProductIndexDocumentManager, ProductIndexDocumentManager>();
    }

    private static void ConfigureCustomServices(this IServiceCollection services)
    {
        services.AddTransient<IIndexTemplateManager, IndexTemplateManager>();
        services.AddTransient<IIndexManager, IndexManager>();
        services.AddScoped<IDocumentManager<Product>, DocumentManager<Product>>();
    }

}
