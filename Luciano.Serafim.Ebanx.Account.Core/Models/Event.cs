namespace Luciano.Serafim.Ebanx.Account.Core.Models;

/// <summary>
/// Define the Eventr entity
/// </summary>
public class Event
{
    public Event(EventOperation operation, double amount, DateTime ocurrence, Account account, Account? destination, Event? originEvent = null)
    {
        Operation = operation;
        Amount = amount;
        Ocurrence = ocurrence;
        Account = account;

        var valid = amount > 0;
        if (!valid)
        {
            throw new Exception("Amount should be higher than 0 (zero)");
        }

        OriginEvent = originEvent;
    }

    /// <summary>
    /// Identifies the Event
    /// </summary>
    public string Id { get; private set; } = string.Empty;

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
    public Account Account { get; private set; }

    /// <summary>
    /// related event that originate the current event
    /// </summary>
    public Event? OriginEvent { get; private set; }
}
