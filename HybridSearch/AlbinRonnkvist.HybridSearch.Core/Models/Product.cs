namespace AlbinRonnkvist.HybridSearch.Core.Models;

public class Product : IModel
{
    public required int Id { get; set; }
    public required string Title { get; set; } = default!;
    public required decimal[] TitleEmbedding { get; set; } = default!;
}
