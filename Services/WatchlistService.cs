using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using sloflix.Data;
using sloflix.Models;

namespace sloflix.Services
{
  public class WatchlistService : IWatchlistService
  {
    private DataContext _dataContext;

    public WatchlistService(DataContext dataContext)
    {
      _dataContext = dataContext;
    }

    public async Task<Watchlist> AddMovieToListAsync(int watchlistId, Movie movie)
    {
      var entity = _dataContext.Attach(movie);
      if (entity.State == EntityState.Added && string.IsNullOrWhiteSpace(movie.Title))
      {
        throw new System.ArgumentException("Movie must have a title", "movie");
      }

      var watchlist = await _dataContext.Watchlists.SingleOrDefaultAsync(list => list.Id == watchlistId);
      if (watchlist == null)
      {
        return null;
      }
      if (watchlist.Movies == null)
      {
        watchlist.Movies = new List<WatchlistItem>();
      }
      watchlist.Movies.Add(new WatchlistItem { Movie = movie, Watchlist = watchlist });
      await _dataContext.SaveChangesAsync();
      return watchlist;
    }

    public async Task<Watchlist> CreateAsync(int watcherId, Watchlist watchlist)
    {
      var name = watchlist.Name;
      if (string.IsNullOrWhiteSpace(name))
        throw new System.ArgumentException("Watchlist must have name");

      watchlist.MovieWatcherId = watcherId;

      await _dataContext.AddAsync(watchlist);
      await _dataContext.SaveChangesAsync();

      return watchlist;
    }

    public void Delete(int watchlistId)
    {
      var watchlist = new Watchlist { Id = watchlistId };
      _dataContext.Attach<Watchlist>(watchlist);
      _dataContext.Remove<Watchlist>(watchlist);
      _dataContext.SaveChanges();
    }

    public async Task<List<Watchlist>> GetAllFromClaimAsync(Claim claim)
    {
      var watcher = await _dataContext.MovieWatchers
          .Include(w => w.Identity)
          .Include(w => w.Watchlists)
          .SingleAsync(w => w.Identity.Id == claim.Value);

      return watcher.Watchlists;
    }

    public Watchlist GetWatchlist(int watchlistId)
    {
      return _dataContext.Watchlists.Single(list => list.Id == watchlistId);
    }

    public void RemoveMovieFromList(int watchlistId, int movieId)
    {
      throw new System.NotImplementedException();
    }

    public async Task<Watchlist> RenameAsync(int watchlistId, string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        throw new System.ArgumentException("Cannot rename to blank name", "name");
      }

      var toRename = await _dataContext.Watchlists.SingleAsync(list => list.Id == watchlistId);
      toRename.Name = name;
      await _dataContext.SaveChangesAsync();

      return toRename;
    }
  }
}
