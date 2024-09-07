using System;
using Luciano.Serafim.Ebanx.Account.Core.Models;

namespace Luciano.Serafim.Ebanx.Account.Core.Abstractions.Services;

/// <summary>
/// Account service
/// </summary>
public interface IAccountService
{
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
    Task<Models.Account> GetAccountById(int accountId);
}
