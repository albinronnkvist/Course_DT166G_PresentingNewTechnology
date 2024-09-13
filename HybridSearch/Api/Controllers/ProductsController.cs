using Microsoft.AspNetCore.Mvc;

namespace AlbinRonnkvist.HybridSearch.Api.Controllers;

[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v{v:apiversion}/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Search()
    {
        await Task.CompletedTask;
        return Ok();
    }
}
