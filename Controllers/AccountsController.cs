using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using AutoMapper;

using sloflix.Data;
using sloflix.Models;
using sloflix.Helpers;
using System.Security.Claims;
using System.Linq;
using sloflix.Services;

namespace sloflix.Controllers
{
  [Route("auth/[controller]")]
  public class AccountsController : Controller
  {
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;
    private readonly DataContext _dataContext;
    private readonly IJwtFactory _jwtFactory;
    private readonly JwtIssuerOptions _jwtOptions;
    private readonly JsonSerializerSettings _serializerSettings;

    public AccountsController(IMapper mapper,
                              UserManager<AppUser> userManager,
                              DataContext dataContext,
                              IJwtFactory jwtFactory,
                              IOptions<JwtIssuerOptions> jwtOptions)
    {
      _mapper = mapper;
      _userManager = userManager;
      _dataContext = dataContext;
      _jwtFactory = jwtFactory;
      _jwtOptions = jwtOptions.Value;

      _serializerSettings = new JsonSerializerSettings
      {
        Formatting = Formatting.Indented
      };
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

      var json = JsonConvert.SerializeObject(_mapper.Map<MovieWatcherDto>(roleResult.Entity), _serializerSettings);

      return new OkObjectResult(json);
      // return new OkResult();
    }

    // POST /auth/accounts/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]AppUserDto credentials)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var identity = await GetClaimsIdentity(credentials.email, credentials.password);
      if (identity == null)
      {
        return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.", ModelState));
      }

      var jwt = await Tokens.GenerateJwt(identity, _jwtFactory, credentials.email, _jwtOptions, _serializerSettings);
      return new OkObjectResult(jwt);
    }

    private async Task<ClaimsIdentity> GetClaimsIdentity(string username, string password)
    {
      if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
      {
        var userToVerify = await _userManager.FindByNameAsync(username);

        if (userToVerify != null)
        {
          // check credentials
          if (await _userManager.CheckPasswordAsync(userToVerify, password))
          {
            return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userToVerify.UserName, userToVerify.Id));
          }
        }
      }

      return await Task.FromResult<ClaimsIdentity>(null);
    }
  }
}
