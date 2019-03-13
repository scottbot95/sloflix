using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using AutoMapper;

using slo_flix.Models;
using slo_flix.Helpers;

namespace slo_flix.Controllers
{
  [Route("auth/[controller]")]
  public class AccountsController : Controller
  {
    private IMapper _mapper;
    private UserManager<AppUser> _userManager;

    public AccountsController(IMapper mapper, UserManager<AppUser> userManager)
    {
      _mapper = mapper;
      _userManager = userManager;
    }


    // POST /auth/accounts
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]UserDto model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var userIdentity = _mapper.Map<AppUser>(model);
      var result = await _userManager.CreateAsync(userIdentity, model.password);

      if (!result.Succeeded)
        return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

      return new JsonResult(userIdentity);
      // return new OkResult();
    }
  }
}
