using AlbinRonnkvist.HybridSearch.Api.Dtos;
using AlbinRonnkvist.HybridSearch.Api.Validators;
using Microsoft.AspNetCore.Mvc;

namespace AlbinRonnkvist.HybridSearch.Api.Controllers;

[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v{v:apiversion}/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        await Task.CompletedTask;
        
        var validationResult = ProductSearchValidator.IsValid(query);
        if (validationResult.IsFailure)
        {
            return BadRequest(validationResult.Error);
        }

        var response = new ProductSearchResponse
        {
            Query = query,
            Hits = 0,
            Products = []
        };

        return Ok(response);
    }
}
