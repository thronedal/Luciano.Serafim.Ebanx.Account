using Luciano.Serafim.Ebanx.Account.Core.Models;
using MediatR;

namespace Luciano.Serafim.Ebanx.Account.Core.UseCases.Events;

/// <summary>
/// WithdrawCommand
/// </summary>
public class WithdrawCommand : IRequest<Response<WithdrawResponse>>
{
    /// <summary>
    /// Origin account Id
    /// </summary>
    public int OriginId { get; set; }

    /// <summary>
    /// Amount to withdraw
    /// </summary>
    public double Amount { get; set; }

    /// <summary>
    /// Converts the base type <see cref="WithdrawCommand"/> to <see cref="Event"/>.
    /// </summary>
    /// <param name="command"><see cref="WithdrawCommand"/></param>
    public static explicit operator Event(WithdrawCommand command)
    {
        return new Event(EventOperation.Withdraw, command.Amount, DateTime.UtcNow, command.OriginId);
    }
}