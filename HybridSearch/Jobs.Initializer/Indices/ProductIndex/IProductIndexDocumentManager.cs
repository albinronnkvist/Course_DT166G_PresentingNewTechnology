using System.Collections.ObjectModel;
using AlbinRonnkvist.HybridSearch.Core.Models;
using CSharpFunctionalExtensions;

namespace AlbinRonnkvist.HybridSearch.Jobs.Initializer.Indices.ProductIndex;

public interface IProductIndexDocumentManager
{
    Task<UnitResult<string>> CreateDocuments(int newVersion, CancellationToken ct);   
}
