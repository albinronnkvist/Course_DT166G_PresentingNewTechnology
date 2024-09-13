using System.Collections.ObjectModel;
using CSharpFunctionalExtensions;

namespace AlbinRonnkvist.HybridSearch.Job.Initializer.Services;

public interface IDocumentManager<TDocument> where TDocument : class
{
    Task<UnitResult<string>> CreateDocuments(string indexName, ReadOnlyCollection<TDocument> documents, CancellationToken ct);    
}
