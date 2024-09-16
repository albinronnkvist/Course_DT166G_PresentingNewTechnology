using AlbinRonnkvist.HybridSearch.Api.Dtos;
using AlbinRonnkvist.HybridSearch.Api.Helpers.Validators;
using AlbinRonnkvist.HybridSearch.Api.Services;
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
    public async Task<IActionResult> Search([FromQuery] ProductSearchRequest request)
    {        
        var sanitizedRequest = ProductSearchValidator.ValidateAndSanitizeRequest(request);
        if (sanitizedRequest.IsFailure)
        {
            return BadRequest(sanitizedRequest.Error);
        }

        var productSearchResult = await _productSearcher.HybridSearch(sanitizedRequest.Value.Query,
            sanitizedRequest.Value.PageNumber, sanitizedRequest.Value.PageSize);
        if (productSearchResult.IsFailure)
        {
            return BadRequest(productSearchResult.Error);
        }

        return Ok(productSearchResult.Value);
    }
}
