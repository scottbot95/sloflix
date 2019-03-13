using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace slo_flix.Models
{
  public class DataContext : DbContext
  {
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Movie> Movies { get; set; }
    public DbSet<UserRating> UserRatings { get; set; }
    public DbSet<Watchlist> Watchlists { get; set; }
    public DbSet<WatchlistItem> WatchlistItems { get; set; }
    public DbSet<User> Users { get; set; }

    public override int SaveChanges()
    {
      var changedEntities = ChangeTracker.Entries();

      foreach (var changedEntity in changedEntities)
      {
        var entity = changedEntity.Entity as Entity;
        if (entity == null) continue;
        switch (changedEntity.State)
        {
          case EntityState.Added:
            entity.OnBeforeInsert(changedEntity);
            break;
          case EntityState.Modified:
            entity.OnBeforeUpdate(changedEntity);
            break;
          case EntityState.Deleted:
            entity.OnBeforeDelete(changedEntity);
            break;
        }
      }

      return base.SaveChanges();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<WatchlistItem>().
        HasKey(m => new { m.MovieId, m.WatchlistId });

      modelBuilder.Entity<UserRating>().
        HasKey(r => new { r.MovieId, r.UserId });
    }
  }

  public abstract class Entity
  {
    /// <summary>
    /// Called before this Entity is inserted into the database
    /// </summary>
    /// <param name="entity"></param>
    public virtual void OnBeforeInsert(EntityEntry entity) { }

    /// <summary>
    /// Called before this existing Entity modified in the database
    /// </summary>
    /// <param name="entity"></param>
    public virtual void OnBeforeUpdate(EntityEntry entity) { }

    /// <summary>
    /// Called before this existing Entity deleted from the database
    /// </summary>
    /// <param name="entity"></param>
    public virtual void OnBeforeDelete(EntityEntry entity) { }
  }
}
