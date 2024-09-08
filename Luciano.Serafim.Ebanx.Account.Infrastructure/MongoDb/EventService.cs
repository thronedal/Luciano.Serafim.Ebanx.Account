using Luciano.Serafim.Ebanx.Account.Core.Abstractions.Services;
using Luciano.Serafim.Ebanx.Account.Core.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace Luciano.Serafim.Ebanx.Account.Infrastructure.MongoDb;

public class EventService : IEventService
{
    private IMongoCollection<Event> eventsCollection;

    public EventService(IOptions<MongoDBSettings> databaseSettings, IMongoClient mongoClient)
    {
        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
        eventsCollection = mongoDatabase.GetCollection<Event>("events");
    }

    /// <inheritdoc/>
    public async Task<bool> InitializeState()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<Event> CreateEvent(Event @event)
    {
        await eventsCollection.InsertOneAsync(@event);
        return @event;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Event>> GetEventsAfter(int accountId, DateTime initialDate)
    {
        return await eventsCollection.Find(e => e.AccountId == accountId && e.Ocurrence >= initialDate).ToListAsync();
    }
}

