using System;
using Luciano.Serafim.Ebanx.Account.Core.Models;

namespace Luciano.Serafim.Ebanx.Account.Core.Abstractions.Services;

public interface IEventService
{
    /// <summary>
    /// Get all events from an account afeter a given date/time
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="initialDate"></param>
    /// <returns></returns>
    Task<IEnumerable<Event>> GetEvetsAfter(int accountId, DateTime initialDate);

    Task<Event> CreateEvent(Models.Account account);
}
