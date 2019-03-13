using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using slo_flix.Models;

namespace slo_flix.Data
{
  public class DataContext : IdentityDbContext
  {
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Movie> Movies { get; set; }
    public DbSet<UserRating> UserRatings { get; set; }
    public DbSet<Watchlist> Watchlists { get; set; }
    public DbSet<WatchlistItem> WatchlistItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<WatchlistItem>().
        HasKey(m => new { m.MovieId, m.WatchlistId });

      modelBuilder.Entity<UserRating>().
        HasKey(r => new { r.MovieId, r.UserId });
    }
  }
}
