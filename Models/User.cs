using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using FluentValidation;

namespace slo_flix.Models
{
  public class User : IdentityUser
  {
    public List<Watchlist> Watchlists { get; set; }
  }

  public class UserDto
  {
    public int id { get; set; }
    public string email { get; set; }
    public string password { get; set; }
  }

  public class UserDtoValidator : AbstractValidator<UserDto>
  {
    public UserDtoValidator()
    {
      RuleFor(dto => dto.email).NotEmpty().EmailAddress().WithMessage("Email can't be empty+foobar");
      RuleFor(dto => dto.email).NotEmpty().WithMessage("Password cannot be empty");
    }
  }
}
