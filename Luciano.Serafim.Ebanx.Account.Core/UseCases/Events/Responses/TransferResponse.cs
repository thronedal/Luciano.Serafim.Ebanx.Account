using System;

namespace Luciano.Serafim.Ebanx.Account.Core.UseCases.Events;

/// <summary>
/// TransferResponse
/// </summary>
public class TransferResponse
{
    public TransferResponse(AccountBalanceResponse origin, AccountBalanceResponse destination)
    {
        Origin = origin;
        Destination = destination;
    }

    /// <summary>
    /// Balance of the origin account
    /// </summary>
    public AccountBalanceResponse Origin { get; internal set; }

    /// <summary>
    /// Balance of the destination account
    /// </summary>
    public AccountBalanceResponse Destination { get; internal set; }
}
