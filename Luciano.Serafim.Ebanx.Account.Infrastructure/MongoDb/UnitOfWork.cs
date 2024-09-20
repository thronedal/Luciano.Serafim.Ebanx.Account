using Luciano.Serafim.Ebanx.Account.Core.Abstractions.Transactions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Luciano.Serafim.Ebanx.Account.Infrastructure.MongoDb;

public class UnitOfWork : IUnitOfWork
{
    private bool isTransactionActive { get { return session.IsInTransaction; } }
    private bool disposed;
    private readonly IClientSessionHandle session;
    private readonly ILogger<UnitOfWork> logger;

    public UnitOfWork(IMongoClient mongoClient, ILogger<UnitOfWork> logger)
    {
        this.logger = logger;
        session = mongoClient.StartSession();
    }

    /// <inheritdoc/>
    public async Task CommitTransactionAsync()
    {
        if (isTransactionActive)
        {
            logger.LogInformation("Commit transaction Id: {id}", session.ServerSession.Id);
            await session.CommitTransactionAsync();
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        // Dispose of unmanaged resources.
        Dispose(true);
        // Suppress finalization.
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    protected virtual void Dispose(bool disposing)
    {
        if (disposed)
        {
            return;
        }

        if (disposing)
        {
            RollbackTransactionAsync().Wait();

            // TODO: dispose managed state (managed objects).

            session.Dispose();
        }

        // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
        // TODO: set large fields to null.

        disposed = true;
    }

    /// <inheritdoc/>
    public async Task RollbackTransactionAsync()
    {
        if (isTransactionActive)
        {
            logger.LogInformation("Abort transaction Id: {id}", session.ServerSession.Id);
            await session.AbortTransactionAsync();
        }
    }

    /// <inheritdoc/>
    public async Task StartTransactionAsync()
    {
        //TODO: add suport to nested transactions

        session.StartTransaction();
        logger.LogInformation("Start transaction Id: {id}", session.ServerSession.Id);
        await Task.CompletedTask;
    }

    public T GetSession<T>()
    {
        return (T)session;
    }
}
