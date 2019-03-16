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

    public async Task<Watchlist> AddMovieToListAsync(Claim userId, int watchlistId, Movie movie)
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

      ThrowIfUnauthorized(userId, watchlist);

      if (watchlist.Movies == null)
      {
        watchlist.Movies = new List<WatchlistItem>();
      }
      watchlist.Movies.Add(new WatchlistItem { Movie = movie, WatchlistId = watchlist.Id });
      await _dataContext.SaveChangesAsync();
      return watchlist;
    }

    public async Task<Watchlist> CreateAsync(Claim userId, Watchlist watchlist)
    {
      var name = watchlist.Name;
      if (string.IsNullOrWhiteSpace(name))
        throw new System.ArgumentException("Watchlist must have name");

      var watcher = await GetWatcherFromClaim(userId);

      if (watcher == null)
      {
        throw new System.Security.Authentication.AuthenticationException("User is not a movie watcher");
      }

      watchlist.MovieWatcherId = watcher.Id;

      await _dataContext.AddAsync(watchlist);
      await _dataContext.SaveChangesAsync();

      return watchlist;
    }

    public void Delete(Claim userId, int watchlistId)
    {
      ThrowIfUnauthorized(userId, watchlistId);
      var watchlist = new Watchlist { Id = watchlistId };
      _dataContext.Attach<Watchlist>(watchlist);
      _dataContext.Remove<Watchlist>(watchlist);
      _dataContext.SaveChanges();
    }

    public async Task<List<Watchlist>> GetAllFromClaimAsync(Claim claim)
    {
      var watcher = await _dataContext.MovieWatchers
          .Include(w => w.Watchlists)
          .Include("Watchlists.Movies")
          .SingleAsync(w => w.IdentityId == claim.Value);

      return watcher.Watchlists.ToList();
    }

    public Task<Watchlist> GetWatchlistAsync(Claim userId, int watchlistId)
    {
      ThrowIfUnauthorized(userId, watchlistId);

      return _dataContext.Watchlists
          .Include("Movies.Movie")
          .SingleAsync(list => list.Id == watchlistId);
    }

    public void RemoveMovieFromList(Claim userId, int watchlistId, int movieId)
    {
      ThrowIfUnauthorized(userId, watchlistId);

      var watchlistItem = new WatchlistItem { WatchlistId = watchlistId, MovieId = movieId };
      _dataContext.Attach<WatchlistItem>(watchlistItem);
      _dataContext.Remove<WatchlistItem>(watchlistItem);
      _dataContext.SaveChanges();
    }

    public async Task<Watchlist> RenameAsync(Claim userId, int watchlistId, string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        throw new System.ArgumentException("Cannot rename to blank name", "name");
      }

      var toRename = await _dataContext.Watchlists.SingleAsync(list => list.Id == watchlistId);
      ThrowIfUnauthorized(userId, toRename);

      toRename.Name = name;
      await _dataContext.SaveChangesAsync();

      return toRename;
    }

    private Task<MovieWatcher> GetWatcherFromClaim(Claim userId)
    {
      return _dataContext.MovieWatchers
          .SingleOrDefaultAsync(w => w.IdentityId == userId.Value);
    }

    private async void ThrowIfUnauthorized(Claim userId, int watchlistId)
    {
      var watcher = await GetWatcherFromClaim(userId);
      ThrowIfUnauthorized(watcher.Id, watchlistId);
    }

    private async void ThrowIfUnauthorized(Claim userId, Watchlist watchlist)
    {
      var watcher = await GetWatcherFromClaim(userId);
      ThrowIfUnauthorized(watcher.Id, watchlist);
    }

    private async void ThrowIfUnauthorized(int watcherId, int watchlistId)
    {
      var watchlist = await _dataContext.Watchlists
        .SingleOrDefaultAsync(wl => wl.Id == watchlistId);
      if (watchlist != null)
      {
        ThrowIfUnauthorized(watcherId, watchlist);
      }
    }

    private void ThrowIfUnauthorized(int watcherId, Watchlist watchlist)
    {
      if (watchlist.MovieWatcherId != watcherId)
      {
        throw new System.Security.Authentication.AuthenticationException(
          "User does not have access to that watchlist");
      }
    }
  }
}
