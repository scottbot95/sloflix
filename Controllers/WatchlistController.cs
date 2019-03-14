using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace sloflix.Controllers
{
  [Authorize(Policy = "ApiUser")]
  [Route("api/[controller]")]
  public class WatchlistController : Controller
  {
    // GET /api/watchlist/test
    [HttpGet("test")]
    public IActionResult GetTest()
    {
      return new OkObjectResult(new { Message = "This is secure data!" });
    }
  }
}
