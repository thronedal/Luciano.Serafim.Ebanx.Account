using System;

namespace Luciano.Serafim.Ebanx.Account.Core.UseCases.Events;

/// <summary>
/// TransferResponse
/// </summary>
public class TransferResponse
{
    /// <summary>
    /// Balance of the origin account
    /// </summary>
    public AccountBalanceResponse? Origin { get; set; }

    /// <summary>
    /// Balance of the destination account
    /// </summary>
    public AccountBalanceResponse? Destination { get; set; }
}
