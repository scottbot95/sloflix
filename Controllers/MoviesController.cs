using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sloflix.Data;

namespace sloflix.Controllers
{
  [Authorize(Policy = "ApiUser")]
  [Route("api/[controller]")]
  public class MoviesController : Controller
  {
    private readonly ClaimsPrincipal _caller;
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public MoviesController(
        IHttpContextAccessor httpContextAccessor,
        DataContext dataContext,
        IMapper mapper)
    {
      _caller = httpContextAccessor.HttpContext.User;
      _dataContext = dataContext;
      _mapper = mapper;
    }
  }
}
