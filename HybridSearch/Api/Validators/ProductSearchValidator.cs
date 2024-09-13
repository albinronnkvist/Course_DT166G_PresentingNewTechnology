using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace AlbinRonnkvist.HybridSearch.Api.Validators;

internal static partial class ProductSearchValidator
{
    internal static UnitResult<string> IsValid(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return UnitResult.Failure<string>("No query provided");
        }

        var allowedCharactersRegex = MyRegex();
        if(!allowedCharactersRegex.IsMatch(query))
        {
            return UnitResult.Failure<string>("Query contains invalid characters");
        }

        return UnitResult.Success<string>();
    }

    [GeneratedRegex(@"^[a-zA-Z0-9\s\-_\.\""'']+$", RegexOptions.Compiled)]
    private static partial Regex MyRegex();
}
