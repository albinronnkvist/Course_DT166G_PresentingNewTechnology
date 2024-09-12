using System.Collections.ObjectModel;
using CSharpFunctionalExtensions;

namespace AlbinRonnkvist.HybridSearch.Jobs.Initializer.Services;

public interface IDocumentManager<TDocument> where TDocument : class
{
    Task<UnitResult<string>> CreateDocuments(string indexName, ReadOnlyCollection<TDocument> documents, CancellationToken ct);    
}
