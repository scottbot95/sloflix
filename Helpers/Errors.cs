using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace sloflix.Helpers
{
  public class Errors
  {
    public static ModelStateDictionary AddErrorsToModelState(IdentityResult identityResult, ModelStateDictionary modelState)
    {
      foreach (var e in identityResult.Errors)
      {
        modelState.TryAddModelError(e.Code, e.Description);
      }
      return modelState;
    }

    public static ModelStateDictionary AddErrorToModelState(string description, ModelStateDictionary modelState)
    {
      return AddErrorToModelState("error", description, modelState);
    }

    public static ModelStateDictionary AddErrorToModelState(string code, string description, ModelStateDictionary modelState)
    {
      modelState.TryAddModelError(code, description);
      return modelState;
    }
  }
}
