using System.Net.Http.Headers;
using AlbinRonnkvist.HybridSearch.Embedding.ApiClients;
using AlbinRonnkvist.HybridSearch.Embedding.Options;
using AlbinRonnkvist.HybridSearch.Embedding.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Polly;
using Polly.Contrib.WaitAndRetry;

namespace AlbinRonnkvist.HybridSearch.Embedding;

public static class ServiceCollectionExtensions
{
    public static void ConfigureEmbeddingProject(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OpenAiEmbeddingApiOptions>(configuration.GetSection(nameof(OpenAiEmbeddingApiOptions)));

        services.AddTransient<IEmbeddingGenerator, OpenAiEmbeddingGenerator>();
        services.AddHttpClient<IEmbeddingApiClient<OpenAiApiClientRequest, OpenAiApiClientResponse>, OpenAiApiClient<OpenAiApiClientRequest, OpenAiApiClientResponse>>((serviceProvider, httpClient) => {
            var openAiEmbeddingApiOptions = configuration.
                GetSection(nameof(OpenAiEmbeddingApiOptions))
                .Get<OpenAiEmbeddingApiOptions>() ?? throw new InvalidOperationException("OpenAiEmbeddingApiOptions is not configured properly.");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", openAiEmbeddingApiOptions.AccessToken);
            httpClient.DefaultRequestHeaders.Add("OpenAI-Organization", openAiEmbeddingApiOptions.OrganizationId);
            httpClient.DefaultRequestHeaders.Add("OpenAI-Project", openAiEmbeddingApiOptions.ProjectId);
            httpClient.BaseAddress = new Uri(openAiEmbeddingApiOptions.BaseUrl);
        }).AddTransientHttpErrorPolicy(x => 
            x.WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 3)));
    }
}
