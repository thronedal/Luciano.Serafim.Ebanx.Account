using System;
using Luciano.Serafim.Ebanx.Account.Core.Models;

namespace Luciano.Serafim.Ebanx.Account.Core.Abstractions.Services;

public interface IEventService
{
    /// <summary>
    /// initialize the app state, for testinf purposes when using in memory storage
    /// </summary>
    /// <returns></returns>
    Task<bool> InitializeState();

    /// <summary>
    /// Get all events from an account afeter a given date/time
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="initialDate"></param>
    /// <returns></returns>
    Task<IEnumerable<Event>> GetEventsAfter(int accountId, DateTime initialDate);

    /// <summary>
    /// persists a new event
    /// </summary>
    /// <param name="event"></param>
    /// <returns></returns>
    Task<Event> CreateEvent(Event @event);
}
