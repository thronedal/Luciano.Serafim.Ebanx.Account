using Luciano.Serafim.Ebanx.Account.Core.Abstractions.Services;
using Luciano.Serafim.Ebanx.Account.Core.Models;
using MongoDB.Driver;
using Luciano.Serafim.Ebanx.Account.Core.Abstractions.Transactions;

namespace Luciano.Serafim.Ebanx.Account.Infrastructure.MongoDb;

public class EventService : IEventService
{
    private IMongoCollection<Event> eventsCollection;
    private readonly IUnitOfWork unitOfWork;

    public EventService(IMongoClient mongoClient, IUnitOfWork unitOfWork)
    {
        var mongoDatabase = mongoClient.GetDatabase(Utils.ACCOUNT_DATABASE_NAME);
        eventsCollection = mongoDatabase.GetCollection<Event>(Utils.EVENT_COLLECTION_NAME);
        this.unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task<bool> InitializeState()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<Event> CreateEvent(Event @event)
    {
        await eventsCollection.InsertOneAsync(unitOfWork.GetSession<IClientSessionHandle>(), @event);
        return @event;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Event>> GetEventsAfter(int accountId, DateTime initialDate)
    {
        return await eventsCollection.Find(unitOfWork.GetSession<IClientSessionHandle>(), e => e.AccountId == accountId && e.Ocurrence >= initialDate).ToListAsync();
    }
}

