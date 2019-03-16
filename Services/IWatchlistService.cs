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
    Task<Watchlist> GetWatchlist(int watcherlistId);
    Task<Watchlist> CreateAsync(int watcherId, Watchlist watchlist);
    Task<Watchlist> RenameAsync(int watchlistId, string name);
    Task<Watchlist> AddMovieToListAsync(int watchlistId, Movie movie);
    void RemoveMovieFromList(int watchlistId, int movieId);
    void Delete(int watchlistId);
  }
}
