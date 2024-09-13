using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace AlbinRonnkvist.HybridSearch.Api.Validators;

public static class ProductSearchValidator
{
    public static UnitResult<string> IsValid(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return UnitResult.Failure<string>("No query provided");
        }

        var allowedCharactersRegex = new Regex(@"^[a-zA-Z0-9_\-\s]+$", RegexOptions.Compiled);
        if(!allowedCharactersRegex.IsMatch(query))
        {
            return UnitResult.Failure<string>("Query contains invalid characters");
        }
        
        return UnitResult.Success<string>();
    }
}
