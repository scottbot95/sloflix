using Microsoft.AspNetCore.Http;

namespace sloflix.Helpers.Extensions
{
  public static class ResponseExtensions
  {
    public static void AddApplicationError(this HttpResponse response, string message)
    {
      try
      {
        response.Headers.Add("Application-Error", message);
        response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
      }
      catch (System.InvalidOperationException e) { }
    }
  }
}
