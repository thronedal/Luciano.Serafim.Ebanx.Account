using System;

namespace Luciano.Serafim.Ebanx.Account.Core.Models;

/// <summary>
/// Define the account Entity
/// </summary>
public class Account
{

    public Account(int id, string accountNumber)
    {
        Id = id;
        AccountNumber = accountNumber;
    }
    /// <summary>
    /// Surrogate Key generated to identify the account, o (zero) indicates a new account
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Account Number
    /// </summary>
    public string AccountNumber { get; private set; }
}
