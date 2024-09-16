using System.Collections.ObjectModel;

namespace AlbinRonnkvist.HybridSearch.Api.Dtos;

public class SearchResponse<TDocument>
{
    public required ReadOnlyCollection<SearchHit<TDocument>> Hits { get; init; }

    public required long TotalHits { get; init; }

    public required long ServerResponseTime { get; init; }
}
