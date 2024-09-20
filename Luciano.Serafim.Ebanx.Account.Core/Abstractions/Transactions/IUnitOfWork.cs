using System;

namespace Luciano.Serafim.Ebanx.Account.Core.Abstractions.Transactions;

/// <summary>
/// interface that defines transaction operations
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// start a transaction
    /// </summary>
    /// <returns></returns>
    Task StartTransactionAsync();

    /// <summary>
    /// Commits the transaction operations
    /// </summary>
    /// <returns></returns>
    Task CommitTransactionAsync();

    /// <summary>
    /// Rolls back the operations
    /// </summary>
    /// <returns></returns>
    Task RollbackTransactionAsync();

    /// <summary>
    /// Gets session object when avaliable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    T GetSession<T>();
}
