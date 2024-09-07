namespace AlbinRonnkvist.HybridSearch.Embedding.ApiClients;

using System.Net.Http.Json;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;

public class OpenAiApiClient<OpenAiApiClientRequest, OpenAiApiClientResponse>(HttpClient httpClient,
    ILogger<OpenAiApiClient<OpenAiApiClientRequest, OpenAiApiClientResponse>> _logger) : IEmbeddingApiClient<OpenAiApiClientRequest, OpenAiApiClientResponse>
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<OpenAiApiClient<OpenAiApiClientRequest, OpenAiApiClientResponse>> _logger = _logger;

    public async Task<Result<OpenAiApiClientResponse, string>> GetEmbedding(OpenAiApiClientRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("", request);
            if (!response.IsSuccessStatusCode)
            {
                var errorDescription = await response.Content.ReadAsStringAsync();
                return Result.Failure<OpenAiApiClientResponse, string>($"Error: {response.StatusCode} - {errorDescription}");
            }

            var result = await response.Content.ReadFromJsonAsync<OpenAiApiClientResponse>();
            _logger.LogWarning("Response: {Response}", result);
            if (result is null)
            {
                return Result.Failure<OpenAiApiClientResponse, string>("Failed to deserialize the response.");
            }

            return Result.Success<OpenAiApiClientResponse, string>(result);
        }
        catch (Exception ex)
        {
            return Result.Failure<OpenAiApiClientResponse, string>($"Exception: {ex.Message}");
        }
    }
}
