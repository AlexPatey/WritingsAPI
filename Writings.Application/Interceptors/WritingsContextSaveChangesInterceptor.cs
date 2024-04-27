using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Writings.Application.Data;
using Writings.Application.Models;

namespace Writings.Application.Interceptors
{
    public class WritingsContextSaveChangesInterceptor : ISaveChangesInterceptor
    {
        public InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            if (eventData.Context is not WritingsContext context)
            {
                return result;
            }

            var tracker = context.ChangeTracker;

            var deleteEntries = tracker.Entries<Writing>()
                .Where(e => e.State == Microsoft.EntityFrameworkCore.EntityState.Deleted);

            foreach (var entry in deleteEntries)
            {
                entry.Property<bool>("Deleted").CurrentValue = true;
                entry.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }

            var updatedEntries = tracker.Entries<Writing>()
                .Where(e => e.State == Microsoft.EntityFrameworkCore.EntityState.Modified);

            foreach (var entry in updatedEntries)
            {
                entry.Property<DateTimeOffset?>("LastEdited").CurrentValue = DateTimeOffset.Now;
            }

            return result;
        }

        public ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = new CancellationToken())
        {
            return ValueTask.FromResult(SavingChanges(eventData, result));
        }
    }
}
