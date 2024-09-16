using Microsoft.Extensions.Options;

namespace AlbinRonnkvist.HybridSearch.Job.Initializer.Options;

public class ProductIndexOptionsValidator : IValidateOptions<ProductIndexOptions>
{
    public ValidateOptionsResult Validate(string? name, ProductIndexOptions options)
    {
        return ValidateOptionsResult.Success;
    }
}
