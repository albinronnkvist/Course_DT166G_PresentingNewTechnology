using Microsoft.Extensions.Options;

namespace AlbinRonnkvist.HybridSearch.Api.Options;

public class ElasticsearchOptionsValidator : IValidateOptions<ElasticsearchOptions>
{
    public ValidateOptionsResult Validate(string? name, ElasticsearchOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.Url))
        {
            return ValidateOptionsResult.Fail("Url is required and cannot be null or empty.");
        }

        if (string.IsNullOrWhiteSpace(options.FingerPrint))
        {
            return ValidateOptionsResult.Fail("FingerPrint is required and cannot be null or empty.");
        }

        if (string.IsNullOrWhiteSpace(options.Username))
        {
            return ValidateOptionsResult.Fail("Username is required and cannot be null or empty.");
        }

        if (string.IsNullOrWhiteSpace(options.Password))
        {
            return ValidateOptionsResult.Fail("Password is required and cannot be null or empty.");
        }

        return ValidateOptionsResult.Success;
    }
}
