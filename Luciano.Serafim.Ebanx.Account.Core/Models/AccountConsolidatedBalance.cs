using System;

namespace Luciano.Serafim.Ebanx.Account.Core.Models;

/// <summary>
/// Controls the calculated balance for an account for the beggining of a given day
/// Should be calculated daily to speedup the balance verification
/// </summary>
public class AccountConsolidatedBalance
{
    public AccountConsolidatedBalance(Account account, DateOnly balanceDate, double balance)
    {
        Account = account;
        BalanceDate = balanceDate;
        Balance = balance;
    }

    /// <summary>
    /// Id
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Account information
    /// </summary>
    public Account Account { get; private set; }

    /// <summary>
    /// Date(UTC) of the balance
    /// </summary>
    public DateOnly BalanceDate { get; private set; }

    /// <summary>
    /// Balance for the Account/Date (0h UTC)
    /// </summary>
    public double Balance { get; private set; }
}
