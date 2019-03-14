using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using FluentValidation;

namespace sloflix.Models
{
  public class AppUser : IdentityUser
  {

  }

  public class AppUserDto
  {
    public int id { get; set; }
    public string email { get; set; }
    public string password { get; set; }
  }

  public class AppUserDtoValidator : AbstractValidator<AppUserDto>
  {
    public AppUserDtoValidator()
    {
      RuleFor(dto => dto.email).NotEmpty().WithMessage("Email can't be empty")
        .EmailAddress().WithMessage("Email must be a valid email address");
      RuleFor(dto => dto.password).NotEmpty().WithMessage("Password cannot be empty");
    }
  }
}
