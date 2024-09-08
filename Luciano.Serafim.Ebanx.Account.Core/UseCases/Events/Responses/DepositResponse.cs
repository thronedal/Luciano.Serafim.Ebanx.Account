namespace Luciano.Serafim.Ebanx.Account.Core.UseCases.Events;

/// <summary>
/// DepositResponse
/// </summary>
public class DepositResponse
{
    public DepositResponse(AccountBalanceResponse destination)
    {
        Destination = destination;
    }

    /// <summary>
    /// Balance of the destination account
    /// </summary>
    public AccountBalanceResponse Destination { get; internal set; }
}
