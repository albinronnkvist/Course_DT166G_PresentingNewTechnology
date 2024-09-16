using System.Collections.ObjectModel;
using Elastic.Clients.Elasticsearch;

namespace AlbinRonnkvist.HybridSearch.Api.Helpers;

// https://plg.uwaterloo.ca/~gvcormac/cormacksigir09-rrf.pdf
// https://medium.com/@devalshah1619/mathematical-intuition-behind-reciprocal-rank-fusion-rrf-explained-in-2-mins-002df0cc5e2a
public static class RRFCombiner
{
    private const int k = 60;

    public static ReadOnlyCollection<TDocument> Combine<TDocument>(SearchResponse<TDocument> keywordSearchResponse,
        SearchResponse<TDocument> semanticSearchResponse, int size)
    {
        var keywordResults = ExtractResults<TDocument>(keywordSearchResponse);
        var semanticResults = ExtractResults<TDocument>(semanticSearchResponse);

        var combinedScores = new Dictionary<string, (TDocument Document, double Score)>();
        AssignRRFScore<TDocument>(combinedScores, keywordResults);
        AssignRRFScore<TDocument>(combinedScores, semanticResults);

        return combinedScores
            .OrderByDescending(x => x.Value.Score)
            .Select(x => x.Value.Document)
            .Take(size)
            .ToList()
            .AsReadOnly();
    }

    private static Dictionary<string, (TDocument Document, int Rank)> ExtractResults<TDocument>(SearchResponse<TDocument> searchResponse)
    {
        var results = new Dictionary<string, (TDocument Document, int Rank)>();
        var hits = searchResponse.Hits;

        for (int i = 0; i < hits.Count(); i++)
        {
            var document = hits.ElementAt(i);
            if (document.Id is null || document.Source is null)
            {
                continue;
            }
            
            results[document.Id] = (document.Source, i + 1);
        }

        return results;
    }

    private static void AssignRRFScore<TDocument>(Dictionary<string, (TDocument Document, double Score)> combinedScores, Dictionary<string, (TDocument Document, int Rank)> results)
    {
        foreach (var result in results)
        {
            var docId = result.Key;
            var document = result.Value.Document;
            var rank = result.Value.Rank;
            double rrfScore = 1.0 / (k + rank);

            if (combinedScores.ContainsKey(docId))
            {
                combinedScores[docId] = (document, combinedScores[docId].Score + rrfScore);
            }
            else
            {
                combinedScores[docId] = (document, rrfScore);
            }
        }
    }
}
