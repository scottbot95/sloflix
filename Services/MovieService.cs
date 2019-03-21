using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using sloflix.Data;
using sloflix.Models;

namespace sloflix.Services
{
  public class MovieService : IMovieService
  {
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public MovieService(DataContext dataContext, IMapper mapper)
    {
      _dataContext = dataContext;
      _mapper = mapper;
    }

    public async Task<Movie> CreateMovieAsync(MovieDto data)
    {
      var movie = _mapper.Map<Movie>(data);
      var title = movie.Title;
      if (string.IsNullOrWhiteSpace(title))
      {
        throw new System.ArgumentException("Title cannot be empty", "data");
      }

      _dataContext.Add(movie);
      await _dataContext.SaveChangesAsync();

      return movie;
    }

    public async Task DeleteAsync(Claim userId, int movieId)
    {
      await ThrowIfNotAdmin(userId);
      var movie = new Movie { Id = movieId };
      await _dataContext.SafeRemoveAsync(movie, (m1, m2) => m1.Id == m2.Id);
    }

    public IQueryable<Movie> GetAllMovies()
    {
      return _dataContext.Movies;
    }

    public async Task RateMovieAsync(Claim userId, int movieId, int rating)
    {
      var watcher = await _dataContext.GetWatcherFromClaim(userId);
      if (watcher == null)
      {
        throw new System.Security.Authentication.AuthenticationException(
          "User is not a movie watcher"
        );
      }

      var userRating = new UserRating { MovieId = movieId, User = watcher, Rating = rating };
      var ratingEntry = _dataContext.Entry(userRating);
      if (rating > 0 && rating < 5)
      {
        // set the user rating
        switch (ratingEntry.State)
        {
          case EntityState.Detached:
          case EntityState.Unchanged:
            ratingEntry.State = EntityState.Modified;
            break;
          default:
            System.Console.WriteLine("**************Unknown State***************");
            System.Console.WriteLine(ratingEntry.State.ToString());
            break;
        }
      }
      else
      {
        ratingEntry.State = EntityState.Deleted;
      }

      await _dataContext.SaveChangesAsync();
    }

    public IQueryable<Movie> SearchMovies(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        throw new System.ArgumentException("Name cannot be blank", "name");
      }

      return _dataContext.Movies.Where(m => m.Title.StartsWith(name));
    }

    private async Task ThrowIfNotAdmin(Claim userId)
    {
      // FIXME Actually check something here...
    }
  }
}
