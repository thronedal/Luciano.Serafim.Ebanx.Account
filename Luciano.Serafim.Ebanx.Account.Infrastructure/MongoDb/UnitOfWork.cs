using Luciano.Serafim.Ebanx.Account.Core.Abstractions.Transactions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Luciano.Serafim.Ebanx.Account.Infrastructure.MongoDb;

/// <summary>
/// Suport ACID for MongoDB
/// </summary>
/// <remarks>
/// MongoDB do not suport nested transactions, if a new transaction is asked inside a existing one, 
/// a counter will be added but no nested transaction will be created. 
/// This counter will be used to control the commit / rollback.
/// </remarks>
public class UnitOfWork : IUnitOfWork
{
    private int transactionLevel = 0;
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
        if (isTransactionActive && transactionLevel == 1)
        {
            logger.LogInformation("Commit transaction Id: {id}", session.ServerSession.Id);
            await session.CommitTransactionAsync();
        }
        transactionLevel--;
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
            while (transactionLevel > 0)
            {
                RollbackTransactionAsync().Wait();
            }

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
        if (isTransactionActive && transactionLevel == 1)
        {
            logger.LogInformation("Abort transaction Id: {id}", session.ServerSession.Id);
            await session.AbortTransactionAsync();
        }
        transactionLevel--;
    }

    /// <inheritdoc/>
    public async Task StartTransactionAsync()
    {
        session.StartTransaction();
        logger.LogInformation("Start transaction Id: {id}", session.ServerSession.Id);

        transactionLevel++;
        await Task.CompletedTask;
    }

    public T GetSession<T>()
    {
        return (T)session;
    }
}
