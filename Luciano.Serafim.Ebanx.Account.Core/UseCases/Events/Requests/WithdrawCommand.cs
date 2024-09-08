using Luciano.Serafim.Ebanx.Account.Core.Models;
using MediatR;

namespace Luciano.Serafim.Ebanx.Account.Core.UseCases.Events;

/// <summary>
/// WithdrawCommand
/// </summary>
public class WithdrawCommand : IRequest<Response<WithdrawResponse>>
{
    public WithdrawCommand(int originId, double amount)
    {
        OriginId = originId;
        Amount = amount;
    }

    /// <summary>
    /// Origin account Id
    /// </summary>
    public int OriginId { get; internal set; }

    /// <summary>
    /// Amount to withdraw
    /// </summary>
    public double Amount { get; internal set; }

    /// <summary>
    /// Converts the base type <see cref="WithdrawCommand"/> to <see cref="Event"/>.
    /// </summary>
    /// <param name="command"><see cref="WithdrawCommand"/></param>
    public static explicit operator Event(WithdrawCommand command)
    {
        return new Event(EventOperation.Withdraw, command.Amount, DateTime.UtcNow, command.OriginId);
    }
}