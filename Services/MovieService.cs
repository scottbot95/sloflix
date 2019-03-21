using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
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
      throw new System.NotImplementedException();
    }

    public IQueryable<Movie> GetAllMovies()
    {
      return _dataContext.Movies;
    }

    public Task RateMovieAsync(Claim userId, int movieId, int rating)
    {
      throw new System.NotImplementedException();
    }

    public IQueryable<Movie> SearchMovies(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        throw new System.ArgumentException("Name cannot be blank", "name");
      }

      return _dataContext.Movies.Where(m => m.Title.StartsWith(name));
    }
  }
}
