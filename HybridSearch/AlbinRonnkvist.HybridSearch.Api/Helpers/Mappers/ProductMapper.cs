using System.Collections.ObjectModel;
using AlbinRonnkvist.HybridSearch.Api.Dtos;

namespace AlbinRonnkvist.HybridSearch.Api.Helpers.Mappers;

public static class ProductMapper
{
    public static ReadOnlyCollection<Product> Map(ReadOnlyCollection<SearchHit<Core.Models.Product>> hits)
    {
        var productDtos = new List<Product>();
        foreach (var hit in hits)
        {
            productDtos.Add(new Product
            {
                Title = hit.Document.Title
            });
        }

        return productDtos.AsReadOnly();
    }

        public static ReadOnlyCollection<Product> Map(ReadOnlyCollection<Core.Models.Product> products)
    {
        var productDtos = new List<Product>();
        foreach (var product in products)
        {
            productDtos.Add(new Product
            {
                Title = product.Title
            });
        }

        return productDtos.AsReadOnly();
    }
}
