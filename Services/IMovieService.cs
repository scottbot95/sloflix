using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using sloflix.Models;

namespace sloflix.Services
{
  public interface IMovieService
  {
    IQueryable<Movie> GetAllMovies();
    IQueryable<Movie> SearchMovies(string title);
    Task RateMovieAsync(Claim userId, int movieId, int rating);
    Task DeleteAsync(Claim userId, int movieId);
    Task<Movie> CreateMovieAsync(MovieDto data);
  }
}
