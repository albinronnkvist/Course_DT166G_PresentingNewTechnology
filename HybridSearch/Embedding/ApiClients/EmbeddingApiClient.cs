namespace AlbinRonnkvist.HybridSearch.Embedding.ApiClients;

using System.Net.Http.Json;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;

public class EmbeddingApiClient : IEmbeddingApiClient
{
    public HttpClient _httpClient;
    private readonly ILogger<EmbeddingApiClient> _logger;

    public EmbeddingApiClient(HttpClient httpClient, ILogger<EmbeddingApiClient> logger)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<Result<EmbeddingApiClientResponse, string>> GetEmbedding(string text)
    {
        var payload = new EmbeddingApiClientRequest
        {
            Inputs = text
        };

        try
        {
            var response = await _httpClient.PostAsJsonAsync("", payload);
            if (!response.IsSuccessStatusCode)
            {
                var errorDescription = await response.Content.ReadAsStringAsync();
                return Result.Failure<EmbeddingApiClientResponse, string>($"Error: {response.StatusCode} - {errorDescription}");
            }

            var result = await response.Content.ReadFromJsonAsync<EmbeddingApiClientResponse>();
            if (result is null)
            {
                return Result.Failure<EmbeddingApiClientResponse, string>("Failed to deserialize the response.");
            }

            return Result.Success<EmbeddingApiClientResponse, string>(result);
        }
        catch (Exception ex)
        {
            return Result.Failure<EmbeddingApiClientResponse, string>($"Exception: {ex.Message}");
        }
    }
}
