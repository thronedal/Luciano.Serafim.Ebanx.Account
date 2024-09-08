namespace Luciano.Serafim.Ebanx.Account.Core.UseCases.Events;

/// <summary>
/// AccountBalanceDto
/// </summary>
public class AccountBalanceResponse
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
