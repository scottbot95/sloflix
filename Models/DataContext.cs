using Microsoft.EntityFrameworkCore;

namespace slo_flix.Models
{
  public class DataContext : DbContext
  {
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
            entity.OnBeforeInsert();
            break;
          case EntityState.Modified:
            entity.OnBeforeUpdate();
            break;
          case EntityState.Deleted:
            entity.OnBeforeDelete();
            break;
        }
      }

      return base.SaveChanges();
    }
  }

  public abstract class Entity
  {
    /// <summary>
    /// Called before this Entity is inserted into the database
    /// </summary>
    public virtual void OnBeforeInsert() { }

    /// <summary>
    /// Called before this existing Entity modified in the database
    /// </summary>
    public virtual void OnBeforeUpdate() { }

    /// <summary>
    /// Called before this existing Entity deleted from the database
    /// </summary>
    public virtual void OnBeforeDelete() { }
  }
}
