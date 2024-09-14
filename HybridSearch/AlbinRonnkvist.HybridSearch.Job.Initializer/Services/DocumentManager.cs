using System.Collections.ObjectModel;
using CSharpFunctionalExtensions;
using Elastic.Clients.Elasticsearch;

namespace AlbinRonnkvist.HybridSearch.Job.Initializer.Services;

public class DocumentManager<TDocument>(ElasticsearchClient elasticsearchClient) : IDocumentManager<TDocument> where TDocument : class
{
    private readonly ElasticsearchClient _elasticsearchClient = elasticsearchClient;

    public async Task<UnitResult<string>> CreateDocuments(string indexName, ReadOnlyCollection<TDocument> documents, CancellationToken ct)
    {
        if(documents.Count is 0)
        {
            return UnitResult.Failure("No documents provided");
        }

        var response = await _elasticsearchClient.IndexManyAsync(documents, indexName, ct);
        if (!response.IsValidResponse)
        {
            return UnitResult.Failure(response.DebugInformation);
        }

        return UnitResult.Success<string>();
    }
}
