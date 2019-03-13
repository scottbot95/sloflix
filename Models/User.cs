using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace slo_flix.Models
{
  public class User : IdentityUser
  {

  }

  public class UserDto
  {
    public int id { get; set; }
    public string email { get; set; }
  }
}
