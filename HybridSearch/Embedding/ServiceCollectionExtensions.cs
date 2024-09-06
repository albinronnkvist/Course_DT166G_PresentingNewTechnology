using AlbinRonnkvist.HybridSearch.Embedding.ApiClients;
using AlbinRonnkvist.HybridSearch.Embedding.Options;
using AlbinRonnkvist.HybridSearch.Embedding.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Contrib.WaitAndRetry;

namespace AlbinRonnkvist.HybridSearch.Embedding;

public static class ServiceCollectionExtensions
{
    public static void ConfigureEmbeddingProject(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmbeddingApiOptions>(configuration.GetSection(nameof(EmbeddingApiOptions)));
        services.AddTransient<IEmbeddingGenerator, EmbeddingGenerator>();
        services.AddHttpClient<IEmbeddingApiClient, EmbeddingApiClient>((serviceProvider, httpClient) => {
            var embeddingApiOptions = serviceProvider.GetRequiredService<IOptions<EmbeddingApiOptions>>().Value;

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {embeddingApiOptions.AccessToken}");
            httpClient.BaseAddress = new Uri($"{embeddingApiOptions.BaseUrl}/{embeddingApiOptions.ModelId}"); // https://api-inference.huggingface.co/models/<MODEL_ID>
        }).AddTransientHttpErrorPolicy(x => 
            x.WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 3)));
    }
}
