namespace Luciano.Serafim.Ebanx.Account.Core.Abstractions.Caching;

/// <summary>
/// defines a interface to MediatR use Invadidation cache behaviour
/// To makes a command utilize the behaviour the ICacheInvalidation must be implemented on the query/command
/// </summary>
public interface ICacheInvalidation
{
    /// <summary>
    /// define a array of cache keys to be invalidated 
    /// </summary>
    public IEnumerable<string> KeysToInvalidate { get; }
}
