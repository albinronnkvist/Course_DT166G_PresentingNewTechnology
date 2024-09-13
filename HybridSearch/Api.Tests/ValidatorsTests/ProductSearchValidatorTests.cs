using AlbinRonnkvist.HybridSearch.Api.Validators;
using FluentAssertions;
using FluentAssertions.Execution;

namespace AlbinRonnkvist.HybridSearch.Api.Tests.ValidatorsTests;

public class ProductSearchValidatorTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("    ")]
    public void IsValid_ShouldReturnFailureWhenQueryIsNullEmptyOrWhitespace(string query)
    {
        var result = ProductSearchValidator.IsValid(query);

        using var assertionScope = new AssertionScope();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("No query provided");
    }

    [Theory]
    [InlineData("validQuery")]
    [InlineData("Valid123")]
    [InlineData("with-dash_and_space")]
    [InlineData("  with space   ")]
    [InlineData("query.with.period")]
    [InlineData("\"quoted\"")]
    [InlineData("'single quoted'")]
    public void IsValid_ShouldReturnSuccessForValidQueries(string query)
    {
        var result = ProductSearchValidator.IsValid(query);

        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData("invalid!query")]         // Contains '!', not allowed
    [InlineData("invalid@query")]         // Contains '@', not allowed
    [InlineData("<script>alert(1)</script>")] // Contains '<', '>', and other disallowed characters
    [InlineData("query#with$special&characters")] // Contains '#', '$', '&', not allowed
    public void IsValid_ShouldReturnFailureForQueriesWithInvalidCharacters(string query)
    {
        var result = ProductSearchValidator.IsValid(query);

        using var assertionScope = new AssertionScope();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Query contains invalid characters");
    }

    [Theory]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("    ", "")]
    public void SanitizeQuery_ShouldReturnEmptyOrWhitespaceWhenQueryIsNullEmptyOrWhitespace(string query, string expected)
    {
        var result = ProductSearchValidator.SanitizeQuery(query);

        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("validQuery", "validQuery")]
    [InlineData("Valid123", "Valid123")]
    [InlineData("with-dash_and_space", "with-dash_and_space")]
    [InlineData("  with space   ", "  with space   ")]
    [InlineData("query.with.period", "query.with.period")]
    [InlineData("\"quoted\"", "\"quoted\"")]
    [InlineData("'single quoted'", "'single quoted'")]
    public void SanitizeQuery_ShouldReturnUnchangedForValidQueries(string query, string expected)
    {
        var result = ProductSearchValidator.SanitizeQuery(query);

        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("invalid!query", "invalid query")]         // '!' replaced by a space
    [InlineData("invalid@query", "invalid query")]         // '@' replaced by a space
    [InlineData("<script>alert(1)</script>", " script alert 1 script ")] // '<', '>', and other disallowed characters replaced by spaces
    [InlineData("query#with$special&characters", "query with special characters")] // '#', '$', '&' replaced by spaces
    public void SanitizeQuery_ShouldReplaceInvalidCharactersWithSpaces(string query, string expected)
    {
        var result = ProductSearchValidator.SanitizeQuery(query);

        result.Should().Be(expected);
    }
}
