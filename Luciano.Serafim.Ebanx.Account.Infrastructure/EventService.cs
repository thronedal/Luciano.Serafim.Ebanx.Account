using System;
using Luciano.Serafim.Ebanx.Account.Core.Abstractions.Services;
using Luciano.Serafim.Ebanx.Account.Core.Models;

namespace Luciano.Serafim.Ebanx.Account.Infrastructure;

public class EventService : IEventService
{
    private readonly List<Event> events = new();

    public EventService()
    {
        var _ = InitializeState().Result;
    }

    /// <inheritdoc/>
    public async Task<bool> InitializeState()
    {
        events.Clear();
        return await Task.FromResult(true);
    }

    /// <inheritdoc/>
    public async Task<Event> CreateEvent(Event @event)
    {
        var e = new Event(@event.Operation, @event.Amount, DateTime.UtcNow, @event.AccountId) { Id = Guid.NewGuid().ToString() };
        events.Add(e);

        return await Task.FromResult(e);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Event>> GetEventsAfter(int accountId, DateTime initialDate)
    {
        return await Task.FromResult(events.Where(e => e.AccountId == accountId && e.Ocurrence >= initialDate).ToList());
    }
}
