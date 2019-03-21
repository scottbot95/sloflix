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
  }
}
