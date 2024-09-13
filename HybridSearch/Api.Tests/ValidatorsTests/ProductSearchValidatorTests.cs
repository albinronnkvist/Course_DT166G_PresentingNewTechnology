using AlbinRonnkvist.HybridSearch.Api.Validators;
using FluentAssertions;

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

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Query contains invalid characters");
    }
}
