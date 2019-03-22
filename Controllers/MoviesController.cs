using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sloflix.Data;
using sloflix.Helpers;
using sloflix.Models;
using sloflix.Services;
using sloflix.Helpers.Extensions;
using Microsoft.EntityFrameworkCore;

namespace sloflix.Controllers
{
  [Authorize(Policy = "ApiUser")]
  [Route("api/[controller]")]
  public class MoviesController : Controller
  {
    private readonly ClaimsPrincipal _caller;
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;
    private readonly IMovieService _service;

    public MoviesController(
        IHttpContextAccessor httpContextAccessor,
        DataContext dataContext,
        IMapper mapper,
        IMovieService service)
    {
      _caller = httpContextAccessor.HttpContext.User;
      _dataContext = dataContext;
      _mapper = mapper;
      _service = service;
    }

    // GET /api/movies/?title
    [HttpGet]
    public async Task<ActionResult<List<MovieDto>>> Search(
      [FromQuery]string title,
      [FromQuery] int page = 1,
      [FromQuery] int pageSize = 20)
    {
      IQueryable<Movie> foundMovies = null;
      if (title == null)
      {
        foundMovies = _service.GetAllMovies();
      }
      else
      {
        foundMovies = _service.SearchMovies(title);
      }

      if (foundMovies == null)
      {
        return new BadRequestObjectResult(Errors.AddErrorToModelState("No Movies Available", ModelState));
      }

      var headers = Response.Headers;
      var paged = foundMovies.GetPaged(page, pageSize);
      var dto = _mapper.Map<List<MovieDto>>(paged.Results);

      headers.Add("Page-Count", paged.PageCount.ToString());
      headers.Add("Pages-Remaining", (paged.PageCount - paged.CurrentPage).ToString());
      headers.Add("Page-Start", paged.FirstRowOnPage.ToString());
      headers.Add("Page-End", paged.LastRowOnPage.ToString());

      return new OkObjectResult(dto);
    }

    // POST /api/movies
    [HttpPost]
    public async Task<ActionResult<MovieDto>> Create([FromBody]MovieDto data)
    {
      if (string.IsNullOrWhiteSpace(data.title))
      {
        return new BadRequestObjectResult(Errors.AddErrorToModelState("Title cannot be blank", ModelState));
      }

      var movie = await _service.CreateMovieAsync(data);

      var dto = _mapper.Map<MovieDto>(movie);
      return new OkObjectResult(dto);
    }

    // DELETE /api/movies/{id}
    [HttpDelete("{movieId}")]
    public async Task<IActionResult> DeleteMovie(int movieId)
    {

      await _service.DeleteAsync(GetUserId(), movieId);
      return NoContent();
    }


    // GET /api/movies/{id}/rating
    [HttpGet("{movieId}/rating")]
    public async Task<ActionResult<UserRatingDto>> GetRating(int movieId)
    {
      var userId = GetUserId();
      var avgRating = await _service.GetAverageRatingAsync(movieId);
      var myRating = await _service.GetUserRatingAsync(userId, movieId);
      return new OkObjectResult(new { avgRating, myRating });
    }

    // PUT /api/movies/{id}/rating
    [HttpPut("{movieId}/rating")]
    public async Task<IActionResult> RateMovie(int movieId, [FromBody]UserRatingDto ratingDto)
    {
      var userId = GetUserId();
      var userRating = await _service.RateMovieAsync(userId, movieId, ratingDto.rating);

      return new OkObjectResult(_mapper.Map<UserRatingDto>(userRating));
    }

    private Claim GetUserId()
    {
      return _caller.Claims.Single(c => c.Type == "id");
    }
  }
}
