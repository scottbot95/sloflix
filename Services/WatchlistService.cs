using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

    public async Task<Watchlist> AddMovieToListAsync(Claim userId, int watchlistId, int movieId)
    {
      var movie = await _dataContext.Movies.SingleOrDefaultAsync(m => m.Id == movieId);
      if (movie == null)
      {
        throw new System.ArgumentException("movieId must correspond to an existing movie", "movieId");
      }

      var watchlist = await _dataContext.Watchlists.SingleOrDefaultAsync(list => list.Id == watchlistId);
      if (watchlist == null)
      {
        return null;
      }

      await ThrowIfUnauthorized(userId, watchlist);

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

      var watcher = await _dataContext.GetWatcherFromClaim(userId);

      if (watcher == null)
      {
        throw new System.Security.Authentication.AuthenticationException("User is not a movie watcher");
      }

      watchlist.MovieWatcherId = watcher.Id;

      await _dataContext.AddAsync(watchlist);
      await _dataContext.SaveChangesAsync();

      return watchlist;
    }

    public async Task Delete(Claim userId, int watchlistId)
    {
      await ThrowIfUnauthorized(userId, watchlistId);
      var watchlist = new Watchlist { Id = watchlistId };
      await _dataContext.SafeRemoveAsync(watchlist, (w1, w2) => w1.Id == w2.Id);
    }

    public async Task<List<Watchlist>> GetAllFromClaimAsync(Claim claim)
    {
      var watcher = await _dataContext.MovieWatchers
          .Include(w => w.Watchlists)
          .Include("Watchlists.Movies")
          .SingleAsync(w => w.IdentityId == claim.Value);

      return watcher.Watchlists.ToList();
    }

    public async Task<Watchlist> GetWatchlistAsync(Claim userId, int watchlistId)
    {
      await ThrowIfUnauthorized(userId, watchlistId);

      return await _dataContext.Watchlists
          .Include("Movies.Movie")
          .SingleAsync(list => list.Id == watchlistId);
    }

    public async Task RemoveMovieFromList(Claim userId, int watchlistId, int movieId)
    {
      await ThrowIfUnauthorized(userId, watchlistId);

      var watchlistItem = new WatchlistItem { WatchlistId = watchlistId, MovieId = movieId };
      await _dataContext.SafeRemoveAsync(watchlistItem,
         (w1, w2) => w1.WatchlistId == w2.WatchlistId && w1.MovieId == w2.MovieId);
    }

    public async Task<Watchlist> RenameAsync(Claim userId, int watchlistId, string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        throw new System.ArgumentException("Cannot rename to blank name", "name");
      }

      var toRename = await _dataContext.Watchlists.SingleAsync(list => list.Id == watchlistId);
      await ThrowIfUnauthorized(userId, toRename);

      toRename.Name = name;
      await _dataContext.SaveChangesAsync();

      return toRename;
    }

    private async Task ThrowIfUnauthorized(Claim userId, int watchlistId)
    {
      var watcher = await _dataContext.GetWatcherFromClaim(userId);
      await ThrowIfUnauthorized(watcher.Id, watchlistId);
    }

    private async Task ThrowIfUnauthorized(Claim userId, Watchlist watchlist)
    {
      var watcher = await _dataContext.GetWatcherFromClaim(userId);
      ThrowIfUnauthorized(watcher.Id, watchlist);
    }

    private async Task ThrowIfUnauthorized(int watcherId, int watchlistId)
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
