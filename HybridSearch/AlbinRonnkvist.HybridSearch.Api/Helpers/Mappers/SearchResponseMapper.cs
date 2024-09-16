using AlbinRonnkvist.HybridSearch.Api.Dtos;
using AlbinRonnkvist.HybridSearch.Core.Models;

namespace AlbinRonnkvist.HybridSearch.Api.Helpers.Mappers;

public static class SearchResponseMapper
{
    public static SearchResponse<TDocument> Map<TDocument>(IReadOnlyCollection<TDocument> documents, long totalHits, long serverResponseTime) where TDocument : IModel
    {
        var hits = new List<SearchHit<TDocument>>();
        foreach (var document in documents)
        {
            hits.Add(new SearchHit<TDocument>
            {
                Id = document.Id,
                Document = document
            });
        }

        return new SearchResponse<TDocument>
        {
            Hits = hits.AsReadOnly(),
            TotalHits = totalHits,
            ServerResponseTime = serverResponseTime
        };
    }
}
