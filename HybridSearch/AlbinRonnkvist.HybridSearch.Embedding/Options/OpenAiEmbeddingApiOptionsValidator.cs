using Microsoft.Extensions.Options;

namespace AlbinRonnkvist.HybridSearch.Embedding.Options;

public class OpenAiEmbeddingApiOptionsValidator : IValidateOptions<OpenAiEmbeddingApiOptions>
{
    public ValidateOptionsResult Validate(string? name, OpenAiEmbeddingApiOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.BaseUrl) || !options.BaseUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            return ValidateOptionsResult.Fail("BaseUrl is required and must start with 'https://'.");
        }

        if (string.IsNullOrWhiteSpace(options.Model))
        {
            return ValidateOptionsResult.Fail("Model is required and cannot be null or empty.");
        }

        if (string.IsNullOrWhiteSpace(options.AccessToken))
        {
            return ValidateOptionsResult.Fail("AccessToken is required and cannot be null or empty.");
        }

        if (string.IsNullOrWhiteSpace(options.OrganizationId))
        {
            return ValidateOptionsResult.Fail("OrganizationId is required and cannot be null or empty.");
        }

        if (string.IsNullOrWhiteSpace(options.ProjectId))
        {
            return ValidateOptionsResult.Fail("ProjectId is required and cannot be null or empty.");
        }

        return ValidateOptionsResult.Success;
    }
}
