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
    public async Task<ActionResult<List<WatchlistDetailsDto>>> Get()
    {
      var userId = _caller.Claims.Single(c => c.Type == "id");
      var watchlists = await _watchlistService.GetAllFromClaimAsync(userId);

      return new OkObjectResult(_mapper.Map<List<WatchlistSummaryDto>>(watchlists));
      // return new OkObjectResult(watchlists);
    }

    // GET /api/watchlist/{id}
    [HttpGet("{watchlistId?}")]
    public async Task<ActionResult<WatchlistDetailsDto>> GetDetails(int watchlistId)
    {
      var watchlist = await _watchlistService.GetWatchlist(watchlistId);
      return new OkObjectResult(_mapper.Map<WatchlistDetailsDto>(watchlist));
    }

    // POST /api/watchlist
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]WatchlistDetailsDto dto)
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

      return new OkObjectResult(_mapper.Map<WatchlistDetailsDto>(watchlist)); ;
    }

    // DELETE /api/watchlist/{id}
    [HttpDelete("{watchlistId?}")]
    public IActionResult Delete(int watchlistId)
    {
      _watchlistService.Delete(watchlistId);
      return new OkResult();
    }

    // PUT /api/watchlist/{id}
    [HttpPut("{watchlistId?}")]
    public async Task<ActionResult<Watchlist>> Put(int watchlistId, [FromBody]MovieDto dto)
    {
      var movie = _mapper.Map<Movie>(dto);
      var watchlist = await _watchlistService.AddMovieToListAsync(watchlistId, movie);
      if (watchlist == null)
      {
        return new BadRequestObjectResult(Errors.AddErrorToModelState("Watchlist doesn't exist", ModelState));
      }

      return new OkObjectResult(watchlist);
    }
  }
}
