namespace AlbinRonnkvist.HybridSearch.Core;

public class Product
{
    public required int Id { get; set; }
    public required string Title { get; set; } = default!;
    public required decimal[] TitleEmbedding { get; set; } = default!;
}
