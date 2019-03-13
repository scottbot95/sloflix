using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace slo_flix.Models
{
  public class User : Entity
  {
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }

    public override void OnBeforeInsert(EntityEntry entity)
    {
      if (entity.OriginalValues["Password"].ToString() !=
          entity.CurrentValues["Password"].ToString())
      {
        // TODO salt and hash the passwords...
      }
    }
  }

  public class UserDto
  {
    public int id { get; set; }
    public string email { get; set; }
  }
}
