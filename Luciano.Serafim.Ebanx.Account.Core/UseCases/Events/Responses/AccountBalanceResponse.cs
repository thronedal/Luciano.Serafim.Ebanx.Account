namespace Luciano.Serafim.Ebanx.Account.Core.UseCases.Events;

/// <summary>
/// AccountBalanceDto
/// </summary>
public class AccountBalanceResponse
{
    public AccountBalanceResponse(int id, double balance)
    {
        Id = id;
        Balance = balance;
    }

    /// <summary>
    /// Account Id
    /// </summary>
    public int Id { get; internal set; }

    /// <summary>
    /// Account Balance
    /// </summary>
    public double Balance { get; internal set; }

}
