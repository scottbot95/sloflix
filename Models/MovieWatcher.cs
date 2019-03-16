using System.Collections.Generic;

namespace sloflix.Models
{
  public class MovieWatcher
  {
    public int Id { get; set; }
    public List<Watchlist> Watchlists { get; set; }
    public string IdentityId { get; set; }
    public AppUser Identity { get; set; }
  }

  public class MovieWatcherDto
  {
    public int id { get; set; }
    public List<WatchlistDetailsDto> watchlists { get; set; }
    public AppUserDto identity { get; set; }
  }
}
