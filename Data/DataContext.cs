using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;
using System;

using sloflix.Models;
using System.Security.Claims;

namespace sloflix.Data
{
  public class DataContext : IdentityDbContext
  {
    public delegate bool entityComparator<T>(T entity1, T entity2) where T : class;

    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Movie> Movies { get; set; }
    public DbSet<UserRating> UserRatings { get; set; }
    public DbSet<Watchlist> Watchlists { get; set; }
    public DbSet<WatchlistItem> WatchlistItems { get; set; }
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<MovieWatcher> MovieWatchers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<WatchlistItem>().
        HasKey(m => new { m.MovieId, m.WatchlistId });

      modelBuilder.Entity<UserRating>().
        HasKey(r => new { r.MovieId, r.MovieWatcherId });
    }

    public async Task SafeRemoveAsync<T>(T entity, entityComparator<T> compare) where T : class
    {
      EntityEntry foundEntry = null;
      foreach (var entry in ChangeTracker.Entries<T>())
      {
        if (compare(entity, entry.Entity))
        {
          foundEntry = entry;
          break;
        }
      }
      if (foundEntry == null)
      {
        foundEntry = this.Attach<T>(entity);
      }
      foundEntry.State = EntityState.Deleted;

      await this.SaveChangesAsync();
    }

    public Task<MovieWatcher> GetWatcherFromClaim(Claim userId)
    {
      return this.MovieWatchers.SingleOrDefaultAsync(mw => mw.IdentityId == userId.Value);
    }
  }
}
