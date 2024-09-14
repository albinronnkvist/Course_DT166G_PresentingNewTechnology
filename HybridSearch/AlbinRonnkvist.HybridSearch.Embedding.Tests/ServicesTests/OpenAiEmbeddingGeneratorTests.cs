using AlbinRonnkvist.HybridSearch.Embedding.ApiClients;
using AlbinRonnkvist.HybridSearch.Embedding.Options;
using AlbinRonnkvist.HybridSearch.Embedding.Services;
using CSharpFunctionalExtensions;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace AlbinRonnkvist.HybridSearch.Embedding.Tests.ServicesTests;

public class OpenAiEmbeddingGeneratorTests
{
    private readonly OpenAiEmbeddingGenerator _sut;
    private readonly IEmbeddingApiClient<OpenAiApiClientRequest, OpenAiApiClientResponse> _embeddingApiClientMock;
    private readonly IOptions<OpenAiEmbeddingApiOptions> _optionsMock;

    public OpenAiEmbeddingGeneratorTests()
    {
        _embeddingApiClientMock = Substitute.For<IEmbeddingApiClient<OpenAiApiClientRequest, OpenAiApiClientResponse>>();
        _optionsMock = Substitute.For<IOptions<OpenAiEmbeddingApiOptions>>();
        _optionsMock.Value.Returns(new OpenAiEmbeddingApiOptions 
            { 
                BaseUrl = "https://test.com",
                AccessToken = "test-access-token",
                OrganizationId = "test-organization-id",
                ProjectId = "test-project-id", 
                Model = "test-model" 
            });

        _sut = new OpenAiEmbeddingGenerator(_optionsMock, _embeddingApiClientMock);
    }

        [Fact]
    public async Task GenerateEmbedding_ShouldPassCorrectRequestToApiClient()
    {
        var text = "test";
        var dimensions = 768;
        var expectedRequest = new OpenAiApiClientRequest
        {
            Input = text,
            Model = _optionsMock.Value.Model,
            Dimensions = dimensions
        };

        _embeddingApiClientMock.GetEmbedding(Arg.Any<OpenAiApiClientRequest>())
            .Returns(Result.Success<OpenAiApiClientResponse, string>(new OpenAiApiClientResponse
        {
            Object = "list",
            Data =
            [
                new OpenAiApiClientResponseData
                {
                    Object = "embedding",
                    Index = 0,
                    Embedding = [0.1m, 0.2m, 0.3m]
                }
            ],
            Model = "test-model",
            Usage = new OpenAiApiClientResponseUsage
            {
                PromptTokens = 1,
                TotalTokens = 1
            }
        }));

        await _sut.GenerateEmbedding(text, dimensions);

        await _embeddingApiClientMock.Received(1).GetEmbedding(Arg.Is<OpenAiApiClientRequest>(r =>
            r.Input == expectedRequest.Input &&
            r.Model == expectedRequest.Model &&
            r.Dimensions == expectedRequest.Dimensions));
    }

    [Fact]
    public async Task GenerateEmbedding_ShouldReturnSuccess_WhenApiClientReturnsSuccess()
    {
        var text = "test";
        var dimensions = 768;
        var expectedEmbedding = new decimal[] {0.1m, 0.2m, 0.3m};

        var embeddingResponse = new OpenAiApiClientResponse
        {
            Object = "list",
            Data =
            [
                new OpenAiApiClientResponseData
                {
                    Object = "embedding",
                    Index = 0,
                    Embedding = expectedEmbedding
                }
            ],
            Model = "test-model",
            Usage = new OpenAiApiClientResponseUsage
            {
                PromptTokens = 1,
                TotalTokens = 1
            }
        };

        _embeddingApiClientMock.GetEmbedding(Arg.Any<OpenAiApiClientRequest>())
            .Returns(Result.Success<OpenAiApiClientResponse, string>(embeddingResponse));

        var result = await _sut.GenerateEmbedding(text, dimensions);

        using var assertionScope = new AssertionScope();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(expectedEmbedding);
    }

    [Fact]
    public async Task GenerateEmbedding_ShouldReturnFailure_WhenApiClientReturnsFailure()
    {
        var text = "test";
        var dimensions = 768;
        var errorMessage = "API error";

        _embeddingApiClientMock.GetEmbedding(Arg.Any<OpenAiApiClientRequest>())
            .Returns(Result.Failure<OpenAiApiClientResponse, string>(errorMessage));

        var result = await _sut.GenerateEmbedding(text, dimensions);

        using var assertionScope = new AssertionScope();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(errorMessage);
    }
}
