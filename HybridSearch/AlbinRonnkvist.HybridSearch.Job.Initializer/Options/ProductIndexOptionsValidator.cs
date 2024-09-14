using Microsoft.Extensions.Options;

namespace AlbinRonnkvist.HybridSearch.Job.Initializer.Options;

public class ProductIndexOptionsValidator : IValidateOptions<ProductIndexOptions>
{
    public ValidateOptionsResult Validate(string? name, ProductIndexOptions options)
    {
        if (options.EmbeddingDimensions <= 0)
        {
            return ValidateOptionsResult.Fail("EmbeddingDimensions must be greater than zero.");
        }

        return ValidateOptionsResult.Success;
    }
}
