using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shared.Contracts.DDD;

namespace Shared.Data.Interceptors;

internal class AuditableEntityInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateAuditableProperties(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateAuditableProperties(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateAuditableProperties(DbContext? context)
    {
        if (context is null) return;

        var entities = context.ChangeTracker
            .Entries<IEntity>()
            .ToList();

        entities.ForEach(x =>
        {
            if (x.State == EntityState.Added) x.Entity.CreatedAt = DateTime.Now;

            if (x.State == EntityState.Added || x.State == EntityState.Modified || x.HasChangedOwnedEntities())
                x.Entity.ModifiedAt = DateTime.Now;
        });
    }
}

internal static class EntityExtensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry is not null &&
            r.TargetEntry.Metadata.IsOwned() &&
            r.TargetEntry.State is EntityState.Added or EntityState.Modified);
}