using AlbinRonnkvist.HybridSearch.Embedding.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AlbinRonnkvist.HybridSearch.Embedding;

public static class ServiceCollectionExtensions
{
    public static void ConfigureEmbeddingProject(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmbeddingApiOptions>(configuration.GetSection(nameof(EmbeddingApiOptions)));
    }
}
