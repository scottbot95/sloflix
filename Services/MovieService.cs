using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using sloflix.Models;

namespace sloflix.Services
{
  public class MovieService : IMovieService
  {
    public Task<Movie> CreateMovie()
    {
      throw new System.NotImplementedException();
    }

    public Task Delete(Claim userId, int movieId)
    {
      throw new System.NotImplementedException();
    }

    public Task<List<Movie>> GetAllMovies()
    {
      throw new System.NotImplementedException();
    }

    public Task RateMovie(Claim userId, int movieId, int rating)
    {
      throw new System.NotImplementedException();
    }

    public Task<List<Movie>> SearchMovies(string names)
    {
      throw new System.NotImplementedException();
    }
  }
}
