using Luciano.Serafim.Ebanx.Account.Core.Models;
using MediatR;

namespace Luciano.Serafim.Ebanx.Account.Core.UseCases.Events;

/// <summary>
/// DepositCommand
/// </summary>
public class DepositCommand : IRequest<Response<DepositResponse>>
{
    public DepositCommand(int destinationId, double amount)
    {
        DestinationId = destinationId;
        Amount = amount;
    }
    
    /// <summary>
    /// Id for destination account
    /// </summary>
    public int DestinationId { get; internal set; }

    /// <summary>
    /// amount to be deposited
    /// </summary>
    public double Amount { get; internal set; }

    /// <summary>
    /// Converts the base type <see cref="DepositCommand"/> to <see cref="Event"/>.
    /// </summary>
    /// <param name="command"><see cref="DepositCommand"/></param>
    public static explicit operator Event(DepositCommand command)
    {
        return new Event(EventOperation.Deposit, command.Amount, DateTime.UtcNow, command.DestinationId);
    }    
}