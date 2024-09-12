using System.Collections.ObjectModel;
using CSharpFunctionalExtensions;
using Elastic.Clients.Elasticsearch;

namespace AlbinRonnkvist.HybridSearch.Jobs.Initializer.Services;

public class DocumentManager<TDocument> : IDocumentManager<TDocument> where TDocument : class
{
    private readonly ElasticsearchClient _elasticsearchClient;

    public DocumentManager(ElasticsearchClient elasticsearchClient)
    {
        _elasticsearchClient = elasticsearchClient;
    }

    public async Task<UnitResult<string>> CreateDocuments(string indexName, ReadOnlyCollection<TDocument> documents, CancellationToken ct)
    {
        if(!documents.Any())
        {
            return UnitResult.Failure<string>("No documents provided");
        }

        var response = await _elasticsearchClient.IndexManyAsync(documents, indexName, ct);
        if (!response.IsValidResponse)
        {
            return UnitResult.Failure<string>(response.DebugInformation);
        }

        return UnitResult.Success<string>();
    }
}
