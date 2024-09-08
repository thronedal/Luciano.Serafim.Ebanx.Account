using System;

namespace Luciano.Serafim.Ebanx.Account.Core.UseCases.Events;

/// <summary>
/// TransferResultDto
/// </summary>
public class TransferResultDto
{
    /// <summary>
    /// Balance of the origin account
    /// </summary>
    public AccountBalanceDto Origin { get; set; }

    /// <summary>
    /// Balance of the destination account
    /// </summary>
    public AccountBalanceDto Destination { get; set; }
}


/// <summary>
/// AccountBalanceDto
/// </summary>
public class AccountBalanceDto
{
    /// <summary>
    /// Account Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Account Balance
    /// </summary>
    public double Balance { get; set; }

}
