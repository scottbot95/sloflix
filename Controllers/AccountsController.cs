using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using AutoMapper;

using slo_flix.Data;
using slo_flix.Models;
using slo_flix.Helpers;

namespace slo_flix.Controllers
{
  [Route("auth/[controller]")]
  public class AccountsController : Controller
  {
    private IMapper _mapper;
    private UserManager<AppUser> _userManager;
    private DataContext _dataContext;

    public AccountsController(IMapper mapper,
                              UserManager<AppUser> userManager,
                              DataContext dataContext)
    {
      _mapper = mapper;
      _userManager = userManager;
      _dataContext = dataContext;
    }


    // POST /auth/accounts
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]AppUserDto model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var userIdentity = _mapper.Map<AppUser>(model);
      var result = await _userManager.CreateAsync(userIdentity, model.password);

      if (!result.Succeeded)
        return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

      var roleResult = await _dataContext.MovieWatchers.AddAsync(new MovieWatcher { IdentityId = userIdentity.Id });
      await _dataContext.SaveChangesAsync();

      return new JsonResult(_mapper.Map<MovieWatcherDto>(roleResult.Entity));
      // return new OkResult();
    }
  }
}
