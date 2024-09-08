using Luciano.Serafim.Ebanx.Account.Core.Models;
using MediatR;

namespace Luciano.Serafim.Ebanx.Account.Core.UseCases.Events;

public class TransferCommand : IRequest<Response<TransferResultDto>>
{
    /// <summary>
    /// Origin account Id
    /// </summary>
    public int OriginId { get; set; }

    /// <summary>
    /// destination account Id
    /// </summary>
    public int DestinationId { get; set; }

    /// <summary>
    /// Operation Amount
    /// </summary>
    public double Amount { get; set; }

    /// <summary>
    /// Converts the base type <see cref="TransferCommand"/> to <see cref="IEnumerable<Event>"/>.
    /// </summary>
    /// <param name="model"><see cref="SimulationRequest"/></param>
    public static explicit operator Event[](TransferCommand command)
    {
        var events = new Event[]
        {
            new Event(EventOperation.OutgoingTransfer, command.Amount, DateTime.UtcNow, command.OriginId),
            new Event(EventOperation.IncommingTransfer, command.Amount, DateTime.UtcNow, command.DestinationId)
        };

        return events;    
        
    }
}