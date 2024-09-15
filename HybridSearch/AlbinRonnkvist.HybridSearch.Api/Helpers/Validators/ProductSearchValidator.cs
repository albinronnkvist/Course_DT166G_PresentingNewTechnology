using System.Text.RegularExpressions;
using AlbinRonnkvist.HybridSearch.Api.Dtos;
using AlbinRonnkvist.HybridSearch.Core.Constants;
using CSharpFunctionalExtensions;

namespace AlbinRonnkvist.HybridSearch.Api.Helpers.Validators;

internal static partial class ProductSearchValidator
{
    internal static Result<ProductSearchSanitizedRequest, string> ValidateAndSanitizeRequest(ProductSearchRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Query))
        {
            return Result.Failure<ProductSearchSanitizedRequest, string>("Query must not be empty.");
        }

        var disAllowedCharactersRegex = DisallowedCharactersRegex();
        var sanitizedQuery = disAllowedCharactersRegex.Replace(request.Query, " ");
        if (string.IsNullOrWhiteSpace(sanitizedQuery))
        {
            return Result.Failure<ProductSearchSanitizedRequest, string>("Query only contained disallowed characters.");
        }

        var pageNumber = request.PageNumber <= 0 
            ? DefaultSearchConstants.DefaultPageNumber 
            : request.PageNumber 
            ?? DefaultSearchConstants.DefaultPageNumber;
        var pageSize = request.PageSize <= 0 
            ? DefaultSearchConstants.DefaultPageSize 
            : request.PageSize 
            ?? DefaultSearchConstants.DefaultPageSize;
        
        return Result.Success<ProductSearchSanitizedRequest, string>(new ProductSearchSanitizedRequest
        {
            Query = sanitizedQuery,
            PageNumber = pageNumber,
            PageSize = pageSize
        });
    }

    [GeneratedRegex(@"[^a-zA-Z0-9\s\-_\.\""'']+", RegexOptions.Compiled)]
    private static partial Regex DisallowedCharactersRegex();
}
