using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using sloflix.Models;

namespace sloflix.Services
{
  public interface IWatchlistService
  {
    Task<List<Watchlist>> GetAllFromClaimAsync(Claim claim);
    Task<Watchlist> GetWatchlistAsync(Claim userId, int watcherlistId);
    Task<Watchlist> CreateAsync(Claim userId, Watchlist watchlist);
    Task<Watchlist> RenameAsync(Claim userId, int watchlistId, string name);
    Task<Watchlist> AddMovieToListAsync(Claim userId, int watchlistId, int movieId);
    void RemoveMovieFromList(Claim userId, int watchlistId, int movieId);
    void Delete(Claim userId, int watchlistId);
  }
}
