using Luciano.Serafim.Ebanx.Account.Core.Exceptions;

namespace Luciano.Serafim.Ebanx.Account.Core.Models;

/// <summary>
/// Define the Eventr entity
/// </summary>
public class Event
{
    public Event(EventOperation operation, double amount, DateTime ocurrence, int accountId)
    {
        Operation = operation;
        Amount = amount;
        Ocurrence = ocurrence;
        AccountId = accountId;

        if (amount <= 0)
        {
            throw new ValueObjectValidationException("400", amount.ToString(), nameof(Amount) );
        }
    }

    /// <summary>
    /// Identifies the Event
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Operation type for the event
    /// </summary>
    public EventOperation Operation { get; private set; }

    /// <summary>
    /// Ammount of the event
    /// </summary>
    public double Amount { get; private set; }

    /// <summary>
    /// Event occurence date (UTC)
    /// </summary>
    public DateTime Ocurrence { get; private set; }

    /// <summary>
    /// Source account for the event
    /// </summary>
    public int AccountId { get; private set; }

    /// <summary>
    /// related event that originate the current event
    /// </summary>
    public string? OriginEventId { get; set; }
}
