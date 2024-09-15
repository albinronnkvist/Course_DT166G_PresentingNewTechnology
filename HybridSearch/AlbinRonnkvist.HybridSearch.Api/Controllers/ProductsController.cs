using AlbinRonnkvist.HybridSearch.Api.Dtos;
using AlbinRonnkvist.HybridSearch.Api.Services;
using AlbinRonnkvist.HybridSearch.Api.Validators;
using Microsoft.AspNetCore.Mvc;

namespace AlbinRonnkvist.HybridSearch.Api.Controllers;

[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v{v:apiversion}/products")]
[ApiController]
public class ProductsController(ISearcher<ProductSearchResponse> productSearcher) : ControllerBase
{
    private readonly ISearcher<ProductSearchResponse> _productSearcher = productSearcher;

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string query)
    {        
        var sanitizedQuery = ProductSearchValidator.SanitizeQuery(query);

        var productSearchResult = await _productSearcher.KeywordSearch(sanitizedQuery);
        if (productSearchResult.IsFailure)
        {
            return BadRequest(productSearchResult.Error);
        }

        return Ok(productSearchResult.Value);
    }
}
