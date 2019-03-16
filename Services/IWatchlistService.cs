using System.Linq;
using System.Threading.Tasks;
using sloflix.Models;

namespace sloflix.Services
{
  public interface IWatchlistService
  {
    IQueryable<Watchlist> GetAllByUserId(int watcherId);
    Watchlist GetWatchlist(int watcherlistId);
    Task<Watchlist> CreateAsync(int watcherId, Watchlist watchlist);
    Task<Watchlist> RenameAsync(int watchlistId, string name);
    void Delete(int watchlistId);
  }
}
