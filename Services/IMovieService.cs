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
    Task DeleteAsync(Claim userId, int movieId);
    Task<Movie> CreateMovieAsync(MovieDto data);

    /// <summary>
    /// Set the rating on a movie based off the claim provided
    /// </summary>
    /// <param name="userId">Claim of user requesting the rating change</param>
    /// <param name="movieId">Id of movie to rate</param>
    /// <param name="rating">The rating to set for the movie (0-5) (0 means remove rating)</param>
    /// <returns></returns>
    Task RateMovieAsync(Claim userId, int movieId, int rating);
  }
}
