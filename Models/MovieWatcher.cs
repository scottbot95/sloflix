using System.Collections.Generic;

namespace slo_flix.Models
{
  public class MovieWatcher
  {
    public int Id { get; set; }
    public List<Watchlist> Watchlists { get; set; }
    public string IdentityId { get; set; }
    public AppUser Identity { get; set; }
  }
}
