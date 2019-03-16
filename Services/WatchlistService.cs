using System.Linq;
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

    public IQueryable<Watchlist> GetAllByUserId(int watcherId)
    {
      return _dataContext.Watchlists.Where(list => list.MovieWatcherId == watcherId);
    }

    public Watchlist GetWatchlist(int watchlistId)
    {
      return _dataContext.Watchlists.Single(list => list.Id == watchlistId);
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
