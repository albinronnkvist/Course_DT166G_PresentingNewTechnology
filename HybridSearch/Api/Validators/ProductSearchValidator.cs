using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace AlbinRonnkvist.HybridSearch.Api.Validators;

internal static partial class ProductSearchValidator
{
    internal static UnitResult<string> IsValid(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return UnitResult.Failure("No query provided");
        }

        var allowedCharactersRegex = AllowedCharactersRegex();
        if(!allowedCharactersRegex.IsMatch(query))
        {
            return UnitResult.Failure("Query contains invalid characters");
        }

        return UnitResult.Success<string>();
    }

    internal static string SanitizeQuery(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return string.Empty;
        }

        var disAllowedCharactersRegex = DisallowedCharactersRegex();
        var sanitizedQuery = disAllowedCharactersRegex.Replace(query, " ");

        return sanitizedQuery;
    }

    [GeneratedRegex(@"^[a-zA-Z0-9\s\-_\.\""'']+$", RegexOptions.Compiled)]
    private static partial Regex AllowedCharactersRegex();

    [GeneratedRegex(@"[^a-zA-Z0-9\s\-_\.\""'']+", RegexOptions.Compiled)]
    private static partial Regex DisallowedCharactersRegex();
}
