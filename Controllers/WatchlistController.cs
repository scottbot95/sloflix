using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using sloflix.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using sloflix.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using sloflix.Services;
using sloflix.Helpers;
using System;
using Newtonsoft.Json;

namespace sloflix.Controllers
{
  [Authorize(Policy = "ApiUser")]
  [Route("api/[controller]")]
  public class WatchlistController : Controller
  {

    private readonly ClaimsPrincipal _caller;
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;
    private readonly IWatchlistService _watchlistService;

    public WatchlistController(
        DataContext dataContext,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        IWatchlistService watchlistService)
    {
      _caller = httpContextAccessor.HttpContext.User;
      _dataContext = dataContext;
      _mapper = mapper;
      _watchlistService = watchlistService;
    }

    // GET /api/watchlist
    [HttpGet]
    public async Task<ActionResult<List<WatchlistDto>>> Get()
    {
      var userId = _caller.Claims.Single(c => c.Type == "id");
      var watchlists = await _watchlistService.GetAllFromClaimAsync(userId);

      return new OkObjectResult(_mapper.Map<List<WatchlistDto>>(watchlists));
    }

    // POST /api/watchlist
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]WatchlistDto dto)
    {
      var name = dto.name;
      if (string.IsNullOrWhiteSpace(name))
      {
        return BadRequest(Errors.AddErrorToModelState("validation_error",
          "Watchlist name cannot be blank", ModelState));
      }

      var watchlist = _mapper.Map<Watchlist>(dto);
      var userId = _caller.Claims.Single(c => c.Type == "id");
      var watcher = await _dataContext.MovieWatchers.SingleOrDefaultAsync(w => w.IdentityId == userId.Value);
      if (watcher == null)
      {
        return BadRequest(Errors.AddErrorToModelState("access_error",
            "User is not a MovieWatcher", ModelState));
      }
      watchlist.MovieWatcherId = watcher.Id;

      await _dataContext.AddAsync(watchlist);
      await _dataContext.SaveChangesAsync();

      return new OkObjectResult(_mapper.Map<WatchlistDto>(watchlist)); ;
    }

    [HttpDelete("{watchlistId}")]
    public IActionResult Delete(int watchlistId)
    {
      _watchlistService.Delete(watchlistId);
      return new OkResult();
    }
  }
}
