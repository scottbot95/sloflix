using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace sloflix.Models
{
  public class Watchlist
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int MovieWatcherId { get; set; }

    public List<WatchlistItem> Movies { get; set; }

    public List<Movie> GetMovies()
    {
      return Movies.Select(m => m.Movie).ToList();
    }
  }

  public class WatchlistDetailsDto
  {
    public int? id { get; set; }
    public string name { get; set; }

    public List<MovieDto> movies { get; set; }
  }

  public class WatchlistSummaryDto
  {
    public int? id { get; set; }
    public string name { get; set; }

    public List<int> movies { get; set; }
  }

  public class WatchlistItem
  {
    public int MovieId { get; set; }
    public Movie Movie { get; set; }

    public int WatchlistId { get; set; }

  }

  public class WatchlistItemDto
  {
    public MovieDto movie { get; set; }
  }
}
