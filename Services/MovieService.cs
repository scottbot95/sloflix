using System;
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

      if (data.tmdbId != null)
      {
        var existing = await _dataContext.Movies.SingleOrDefaultAsync(m => m.TMDbId == data.tmdbId);
        if (existing != null)
        {
          return existing;
        }
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

    public async Task<double> GetAverageRatingAsync(int movieId)
    {
      var movie = await _dataContext.Movies.Include(m => m.UserRatings).SingleOrDefaultAsync(m => m.Id == movieId);
      int sum = 0;
      int count = 0;
      foreach (var rating in movie.UserRatings)
      {
        sum += rating.Rating;
        ++count;
      }

      return ((double)sum) / Math.Max(count, 1);
    }

    public async Task<int> GetUserRatingAsync(Claim userId, int movieId)
    {
      var watcher = await _dataContext.GetWatcherFromClaim(userId);

      var rating = await _dataContext.UserRatings.SingleOrDefaultAsync(r => r.MovieWatcherId == watcher.Id && r.MovieId == movieId);

      return rating.Rating;
    }



    public async Task<UserRating> RateMovieAsync(Claim userId, int movieId, int rating)
    {
      var watcher = await _dataContext.GetWatcherFromClaim(userId);
      if (watcher == null)
      {
        throw new System.Security.Authentication.AuthenticationException(
          "User is not a movie watcher"
        );
      }

      var entity = await _dataContext.UserRatings
        .SingleOrDefaultAsync(r => r.MovieId == movieId && r.MovieWatcherId == watcher.Id);
      if (rating > 0 && rating <= 5)
      {
        if (entity != null)
        {
          entity.Rating = rating;
        }
        else
        {
          entity = new UserRating { MovieId = movieId, User = watcher, Rating = rating };
          _dataContext.Attach(entity);
        }
      }
      else if (entity != null)
      {
        await _dataContext.SafeRemoveAsync(entity,
          (a, b) => a.MovieId == b.MovieId && a.MovieWatcherId == b.MovieWatcherId);
      }

      await _dataContext.SaveChangesAsync();

      return entity;
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
