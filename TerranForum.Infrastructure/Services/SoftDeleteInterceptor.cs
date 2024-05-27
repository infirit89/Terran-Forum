using Microsoft.EntityFrameworkCore.Diagnostics;
using TerranForum.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace TerranForum.Infrastructure.Services
{
    public sealed class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            Console.WriteLine("cum cum cum cum cum cum");
            if(eventData.Context is null)
                return base.SavingChangesAsync(eventData, result, cancellationToken);

            IEnumerable<EntityEntry<ISoftDeletableEntity>> deletedEntities = 
                eventData
                .Context
                .ChangeTracker
                .Entries<ISoftDeletableEntity>()
                .Where(e => e.State == EntityState.Deleted);

            foreach (EntityEntry<ISoftDeletableEntity> deletedEntity in deletedEntities) 
            {
                deletedEntity.State = EntityState.Modified;
                deletedEntity.Entity.IsDeleted = true;
                deletedEntity.Entity.DeletedAt = DateTimeOffset.UtcNow;
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
