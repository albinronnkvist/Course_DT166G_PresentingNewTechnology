using AlbinRonnkvist.HybridSearch.WebApp.ApiClients;
using AlbinRonnkvist.HybridSearch.WebApp.Options;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Polly;
using Polly.Contrib.WaitAndRetry;

namespace AlbinRonnkvist.HybridSearch.WebApp;

internal static class ServiceCollectionExtensions
{
    internal static void ConfigureWebAppProject(this IServiceCollection services, WebAssemblyHostConfiguration hostConfiguration) 
    {
        services.Configure<HybridSearchApiOptions>(hostConfiguration.GetSection(nameof(HybridSearchApiOptions)));

        services.AddHttpClient<IHybridSearchApiClient, HybridSearchApiClient>((serviceProvider, httpClient) => {
            var hybridSearchApiOptions = hostConfiguration.
                GetSection(nameof(HybridSearchApiOptions))
                .Get<HybridSearchApiOptions>() ?? throw new InvalidOperationException("HybridSearchApiOptions is not configured properly.");

            httpClient.BaseAddress = new Uri(hybridSearchApiOptions.BaseUrl);
        }).AddTransientHttpErrorPolicy(x => 
            x.WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 3)));
    }
}
