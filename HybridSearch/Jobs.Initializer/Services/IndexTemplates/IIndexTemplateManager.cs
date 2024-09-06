using CSharpFunctionalExtensions;
using Elastic.Clients.Elasticsearch.IndexManagement;

namespace AlbinRonnkvist.HybridSearch.Jobs.Initializer.Services.IndexTemplates;

public interface IIndexTemplateManager
{
    Task<UnitResult<string>> UpsertIndexTemplate(PutIndexTemplateRequest request, CancellationToken ct);
}
