namespace Luciano.Serafim.Ebanx.Account.Core.Models;

public enum EventOperation
{
    /// <summary>
    /// Cash-in a amount into a account
    /// </summary>
    Deposit,

    /// <summary>
    /// Cash-out a amount from a account
    /// </summary>
    Withdraw,

    /// <summary>
    /// incomming transfer to a account
    /// </summary>
    IncommingTransfer,

    /// <summary>
    /// Outgoing transfer from a account
    /// </summary>
    OutgoingTransfer
}
