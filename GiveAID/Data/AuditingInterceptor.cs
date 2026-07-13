using GiveAID.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace GiveAID.Data;

public class AuditingInterceptor : SaveChangesInterceptor
{
    // Override async method
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken ct = default)
    {
        UpdateTimestamps(eventData.Context);
        return base.SavingChangesAsync(eventData, result, ct);
    }

    // Override sync method (just in case someone calls .SaveChanges() instead of Async)
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        UpdateTimestamps(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    private static void UpdateTimestamps(DbContext? context)
    {
        if (context == null) return;

        var entries = context.ChangeTracker.Entries();
        var utcNow = DateTimeOffset.UtcNow;

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                // Handle Entities that just got Added
                case EntityState.Added:
                {
                    if (entry.Entity is IHasCreatedAt createdEntity)
                    {
                        // Only overwrite if the date is close to now (default initialization). 
                        // This allows the DbSeeder to set historical CreatedAt dates for charts.
                        if (Math.Abs((utcNow - createdEntity.CreatedAt).TotalMinutes) < 5)
                        {
                            createdEntity.CreatedAt = utcNow;
                        }
                    }

                    break;
                }
                // Handle Entities that got Modified
                case EntityState.Modified:
                {
                    if (entry.Entity is IHasUpdatedAt updatedEntity)
                    {
                        updatedEntity.UpdatedAt = utcNow;
                    }

                    break;
                }
            }
        }
    }
}
