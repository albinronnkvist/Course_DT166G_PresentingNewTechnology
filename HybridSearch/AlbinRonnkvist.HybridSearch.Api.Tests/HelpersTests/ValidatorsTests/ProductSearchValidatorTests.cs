using AlbinRonnkvist.HybridSearch.Api.Dtos;
using AlbinRonnkvist.HybridSearch.Api.Helpers.Validators;
using FluentAssertions;
using FluentAssertions.Execution;

namespace AlbinRonnkvist.HybridSearch.Api.Tests.HelpersTests.ValidatorsTests;

public class ProductSearchValidatorTests
{
    [Theory]
    [InlineData(null, "Query must not be empty.")]
    [InlineData("", "Query must not be empty.")]
    [InlineData("   ", "Query must not be empty.")]
    public void ValidateAndSanitizeRequest_ShouldReturnErrorWhenQueryIsNullEmptyOrWhitespace(string query, string expectedError)
    {
        var request = new ProductSearchRequest { Query = query };

        var result = ProductSearchValidator.ValidateAndSanitizeRequest(request);

        using var assertionScope = new AssertionScope();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(expectedError);
    }

    [Theory]
    [InlineData("validQuery", "validQuery")]
    [InlineData("Valid123", "Valid123")]
    [InlineData("with-dash_and_space", "with-dash_and_space")]
    [InlineData("  with space   ", "  with space   ")]
    [InlineData("query.with.period", "query.with.period")]
    [InlineData("\"quoted\"", "\"quoted\"")]
    [InlineData("'single quoted'", "'single quoted'")]
    public void ValidateAndSanitizeRequest_ShouldReturnUnchangedForValidQueries(string query, string expectedQuery)
    {
        var request = new ProductSearchRequest { Query = query };

        var result = ProductSearchValidator.ValidateAndSanitizeRequest(request);

        using var assertionScope = new AssertionScope();
        result.IsSuccess.Should().BeTrue();
        result.Value.Query.Should().Be(expectedQuery);
    }

    [Theory]
    [InlineData("invalid!query", "invalid query")]         // '!' replaced by a space
    [InlineData("invalid@query", "invalid query")]         // '@' replaced by a space
    [InlineData("<script>alert(1)</script>", " script alert 1 script ")] // '<', '>', and other disallowed characters replaced by spaces
    [InlineData("query#with$special&characters", "query with special characters")] // '#', '$', '&' replaced by spaces
    public void ValidateAndSanitizeRequest_ShouldReplaceInvalidCharactersWithSpaces(string query, string expectedSanitizedQuery)
    {
        var request = new ProductSearchRequest { Query = query };

        var result = ProductSearchValidator.ValidateAndSanitizeRequest(request);

        using var assertionScope = new AssertionScope();
        result.IsSuccess.Should().BeTrue();
        result.Value.Query.Should().Be(expectedSanitizedQuery);
    }

    [Theory]
    [InlineData(null, null, 0, 10)] // Default values
    [InlineData(-2, -15, 0, 10)]    // Negative values not allowed
    [InlineData(2, 15, 2, 15)]      // Provided values
    public void ValidateAndSanitizeRequest_ShouldSetDefaultPageNumberAndPageSizeIfNotProvided(
        int? pageNumber, int? pageSize, int expectedPageNumber, int expectedPageSize)
    {
        var request = new ProductSearchRequest
        {
            Query = "validQuery",
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = ProductSearchValidator.ValidateAndSanitizeRequest(request);

        using var assertionScope = new AssertionScope();
        result.IsSuccess.Should().BeTrue();
        result.Value.PageNumber.Should().Be(expectedPageNumber);
        result.Value.PageSize.Should().Be(expectedPageSize);
    }

    [Theory]
    [InlineData("! @ #  ", "Query only contained disallowed characters.")]
    [InlineData("<>", "Query only contained disallowed characters.")]
    public void ValidateAndSanitizeRequest_ShouldReturnErrorWhenQueryContainsOnlyDisallowedCharacters(
        string query, string expectedError)
    {
        var request = new ProductSearchRequest { Query = query };

        var result = ProductSearchValidator.ValidateAndSanitizeRequest(request);

        using var assertionScope = new AssertionScope();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(expectedError);
    }
}
