namespace AlbinRonnkvist.HybridSearch.Embedding.ApiClients;

using System.Net.Http.Json;
using CSharpFunctionalExtensions;

public class EmbeddingApiClient : IEmbeddingApiClient
{
    public HttpClient _httpClient;

    public EmbeddingApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<EmbeddingApiClientResponse, string>> GetEmbedding(string text)
    {
        var payload = new EmbeddingApiClientRequest
        {
            Text = text
        };

        try
        {
            var response = await _httpClient.PostAsJsonAsync("", payload);

            if (!response.IsSuccessStatusCode)
            {
                return Result.Failure<EmbeddingApiClientResponse, string>($"Error: {response.StatusCode} - {response.ReasonPhrase}");
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
