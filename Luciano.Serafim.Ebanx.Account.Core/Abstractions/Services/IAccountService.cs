using System;
using Luciano.Serafim.Ebanx.Account.Core.Models;

namespace Luciano.Serafim.Ebanx.Account.Core.Abstractions.Services;

/// <summary>
/// Account service
/// </summary>
public interface IAccountService
{

    /// <summary>
    /// initialize the app state, for testing purposes when using in memory storage
    /// </summary>
    /// <returns></returns>
    Task<bool> InitializeState();

    /// <summary>
    /// return the last consolidated balance for the account
    /// </summary>
    /// <param name="accountId">account Id</param>
    /// <returns></returns>
    Task<AccountConsolidatedBalance> GetLastConsolidatedBalance(int accountId);

    
    /// <summary>
    /// return account info by its Id
    /// </summary>
    /// <param name="accountId">account Id</param>
    /// <returns></returns>
    Task<Models.Account?> GetAccountById(int accountId);

    /// <summary>
    /// presists a new account
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    Task<Models.Account> CreateAccount(Models.Account account);

    /// <summary>
    /// Check if an account exists
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns></returns>
    Task<bool> AccountExists(int accountId);

    /// <summary>
    /// consolidate balance for the day
    /// </summary>
    /// <param name="id"></param>
    /// <param name="v"></param>
    /// <returns></returns>
    Task<AccountConsolidatedBalance> ConsolidateBalance(Models.Account account, DateOnly date, double balance);
}
