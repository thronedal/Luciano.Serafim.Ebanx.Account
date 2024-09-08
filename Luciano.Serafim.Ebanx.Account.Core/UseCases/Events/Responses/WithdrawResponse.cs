namespace Luciano.Serafim.Ebanx.Account.Core.UseCases.Events;

/// <summary>
/// TransferResultDto
/// </summary>
public class WithdrawResponse
{
    /// <summary>
    /// Balance of the origin account
    /// </summary>
    public AccountBalanceResponse? Origin { get; set; }
}
