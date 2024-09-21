using System.Net.Http.Json;
using AlbinRonnkvist.HybridSearch.WebApp.Dtos;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.WebUtilities;

namespace AlbinRonnkvist.HybridSearch.WebApp.ApiClients;

public class HybridSearchApiClient(HttpClient httpClient) : IHybridSearchApiClient
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<Result<ProductSearchResponse, string>> ProductSearch(ProductSearchRequest request)
    {
        try
        {
            var requestUrl = BuildProductSearchUrl(request);
            var response = await _httpClient.GetAsync(requestUrl);
            if (!response.IsSuccessStatusCode)
            {
                var errorDescription = await response.Content.ReadAsStringAsync();
                return Result.Failure<ProductSearchResponse, string>($"Error: {response.StatusCode} - {errorDescription}");
            }

            var result = await response.Content.ReadFromJsonAsync<ProductSearchResponse>();
            if (result is null)
            {
                return Result.Failure<ProductSearchResponse, string>("Failed to deserialize the response.");
            }

            return Result.Success<ProductSearchResponse, string>(result);
        }
        catch (Exception ex)
        {
            return Result.Failure<ProductSearchResponse, string>($"Exception: {ex.Message}");
        }
    }

    private static string BuildProductSearchUrl(ProductSearchRequest request)
    {
        var queryParams = new Dictionary<string, string?>();
        if (!string.IsNullOrWhiteSpace(request.Query))
        {
            queryParams.Add("Query", request.Query);
        }

        if (request.PageNumber.HasValue)
        {
            queryParams.Add("PageNumber", request.PageNumber.Value.ToString());
        }

        if (request.PageSize.HasValue)
        {
            queryParams.Add("PageSize", request.PageSize.Value.ToString());
        }

        return QueryHelpers.AddQueryString("products", queryParams);
    }
}
