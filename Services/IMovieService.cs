using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using sloflix.Models;

namespace sloflix.Services
{
  public interface IMovieService
  {
    Task<List<Movie>> GetAllMovies();
    Task<List<Movie>> SearchMovies(string names);
    Task RateMovie(Claim userId, int movieId, int rating);
    Task Delete(Claim userId, int movieId);
    Task<Movie> CreateMovie();
  }
}
