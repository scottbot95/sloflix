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
  public class WatchlistsController : Controller
  {

    private readonly ClaimsPrincipal _caller;
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;
    private readonly IWatchlistService _watchlistService;

    public WatchlistsController(
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

    // GET /api/watchlists
    [HttpGet]
    /// <summary>
    /// Returns a list of all watchlists currently created by the user
    /// </summary>
    public async Task<ActionResult<List<WatchlistSummaryDto>>> Get()
    {
      var userId = GetUserId();
      var watchlists = await _watchlistService.GetAllFromClaimAsync(userId);

      return new OkObjectResult(_mapper.Map<List<WatchlistSummaryDto>>(watchlists));
      // return new OkObjectResult(watchlists);
    }

    // GET /api/watchlists/{id}
    [HttpGet("{watchlistId}")]
    /// <summary>
    /// Returns details (eager load movie details) for a specific watchlist
    /// </summary>
    public async Task<ActionResult<WatchlistDetailsDto>> GetDetails(int watchlistId)
    {
      var watchlist = await _watchlistService.GetWatchlistAsync(GetUserId(), watchlistId);
      return new OkObjectResult(_mapper.Map<WatchlistDetailsDto>(watchlist));
    }

    // POST /api/watchlists
    [HttpPost]
    /// <summary>
    /// Create a new watchlist for the logged in user with provided details
    /// Optionally can take a list of movies to instantiate the list with
    /// </summary>
    public async Task<IActionResult> Post([FromBody]WatchlistDetailsDto dto)
    {
      var name = dto.name;
      if (string.IsNullOrWhiteSpace(name))
      {
        return BadRequest(Errors.AddErrorToModelState("validation_error",
          "Watchlist name cannot be blank", ModelState));
      }

      var watchlist = _mapper.Map<Watchlist>(dto);
      var userId = GetUserId();
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

    // DELETE /api/watchlists/{id}
    [HttpDelete("{watchlistId}")]
    /// <summary>
    /// Delete the specified watchlist
    /// </summary>
    public IActionResult Delete(int watchlistId)
    {
      _watchlistService.Delete(GetUserId(), watchlistId);
      return new OkResult();
    }

    // PUT /api/watchlists/{id}
    [HttpPut("{watchlistId}")]
    /// <summary>
    /// Add a movie to an existing watchlist
    /// </summary>
    public async Task<ActionResult<Watchlist>> AddMovie(int watchlistId, [FromBody]MovieDto dto)
    {
      var movie = _mapper.Map<Movie>(dto);
      var watchlist = await _watchlistService.AddMovieToListAsync(GetUserId(), watchlistId, movie);
      if (watchlist == null)
      {
        return new BadRequestObjectResult(Errors.AddErrorToModelState("Watchlist doesn't exist", ModelState));
      }

      return new OkObjectResult(watchlist);
    }

    // DELETE /api/watchlists/{watchlistId}/{movieId}
    [HttpDelete("{watchlistId}/{movieId}")]
    /// <summary>
    /// Remove a movie from the watchlist
    /// </summary>
    public IActionResult RemoveMovie(int watchlistId, int movieId)
    {
      _watchlistService.RemoveMovieFromList(GetUserId(), watchlistId, movieId);
      return Ok();
    }

    private Claim GetUserId()
    {
      return _caller.Claims.Single(c => c.Type == "id");
    }
  }
}
