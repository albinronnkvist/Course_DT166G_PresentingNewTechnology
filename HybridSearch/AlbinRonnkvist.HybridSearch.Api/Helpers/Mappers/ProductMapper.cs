using System.Collections.ObjectModel;
using AlbinRonnkvist.HybridSearch.Core.Models;

namespace AlbinRonnkvist.HybridSearch.Api.Helpers.Mappers;

public static class ProductMapper
{
    public static ReadOnlyCollection<Dtos.Product> Map(IReadOnlyCollection<Product> products)
    {
        var productDtos = new List<Dtos.Product>();
        foreach (var product in products)
        {
            productDtos.Add(new Dtos.Product
            {
                Title = product.Title
            });
        }

        return productDtos.AsReadOnly();
    }
}
