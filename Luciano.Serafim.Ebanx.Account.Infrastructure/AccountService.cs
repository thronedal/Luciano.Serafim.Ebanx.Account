using System;
using Luciano.Serafim.Ebanx.Account.Core.Abstractions.Services;
using Luciano.Serafim.Ebanx.Account.Core.Models;

namespace Luciano.Serafim.Ebanx.Account.Infrastructure;

public class AccountService : IAccountService
{
    private readonly List<Core.Models.Account> accounts = new();
    private readonly List<AccountConsolidatedBalance> consolidatedBalances = new();

    public AccountService()
    {
        var _ = InitializeState().Result;
    }

    /// <inheritdoc/>
    public async Task<bool> InitializeState()
    {
        accounts.Clear();
        consolidatedBalances.Clear();

        //creates accounts from 1 to 99 and 300
        accounts.AddRange(Enumerable.Range(1, 99).Select(i => new Account.Core.Models.Account(i, i.ToString())).ToList());
        accounts.Add(new Core.Models.Account(300, "300"));

        //creates consolidated balance for today, with the balance equal to the account id    
        consolidatedBalances.AddRange(accounts.Select(a => new AccountConsolidatedBalance(a, DateOnly.FromDateTime(DateTime.UtcNow), a.Id)));

        return await Task.FromResult(true);
    }

    /// <inheritdoc/>
    public async Task<bool> AccountExists(int accountId)
    {
        return await Task.FromResult(accounts.Exists(a => a.Id == accountId));
    }

    /// <inheritdoc/>
    public async Task<AccountConsolidatedBalance> ConsolidateBalance(Core.Models.Account account, DateOnly date, double balance)
    {
        var consolidated = new AccountConsolidatedBalance(account, DateOnly.FromDateTime(DateTime.UtcNow), balance);
        consolidatedBalances.Add(consolidated);

        return await Task.FromResult(consolidated);
    }

    /// <inheritdoc/>
    public async Task<Core.Models.Account> CreateAccount(Core.Models.Account account)
    {
        accounts.Add(account);

        return await Task.FromResult(account);
    }

    /// <inheritdoc/>
    public async Task<Core.Models.Account?> GetAccountById(int accountId)
    {
        return await Task.FromResult(accounts.Where(a => a.Id == accountId).FirstOrDefault());
    }

    /// <inheritdoc/>
    public async Task<AccountConsolidatedBalance> GetLastConsolidatedBalance(int accountId)
    {
        return await Task.FromResult(consolidatedBalances.Where(c => c.Account.Id == accountId 
                                         && c.BalanceDate == consolidatedBalances.Where(c => c.Account.Id == accountId).Max(m => m.BalanceDate))
                                   .FirstOrDefault(new AccountConsolidatedBalance(new Core.Models.Account(accountId, accountId.ToString()), DateOnly.MinValue, 0)));
                            
    }
}
