using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sloflix.Models
{
  public class Watchlist
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int MovieWatcherId { get; set; }

    public List<WatchlistItem> Movies { get; set; }
  }

  public class WatchlistDto
  {
    public int id { get; set; }
    public string name { get; set; }

    public List<WatchlistItemDto> movies { get; set; }
  }

  public class WatchlistItem
  {
    public int MovieId { get; set; }
    public Movie Movie { get; set; }

    public int WatchlistId { get; set; }
    public Watchlist Watchlist { get; set; }
  }

  public class WatchlistItemDto
  {
    public MovieDto movie { get; set; }
  }
}
