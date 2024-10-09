using Luciano.Serafim.Ebanx.Account.Core.Exceptions.Abstractions;

namespace Luciano.Serafim.Ebanx.Account.Core.Exceptions;

/// <summary>
/// Exception raised when the resource locking fail 
/// </summary>
public class ResourceLockingTimeOutException : BaseException, IConflictException
{
    /// <inheritdoc/>
    public ResourceLockingTimeOutException(string message) : base("409", message)
    {

    }

    /// <inheritdoc/>
    public ResourceLockingTimeOutException(string message, Exception innerException) : base("409", message, innerException)
    {

    }
}