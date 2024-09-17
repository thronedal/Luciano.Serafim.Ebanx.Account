using System;
using Microsoft.Extensions.Caching.Distributed;

namespace Luciano.Serafim.Ebanx.Account.Core.Abstractions.Caching;

/// <summary>
/// defines a interface for MediatR use the caching behaviour
/// adding this interface to a Query or Command "automagically" makes it cacheable
/// </summary>
public interface ICacheable
{
    /// <summary>
    /// cache key to be utilized
    /// </summary>
    public string CacheKey { get; }

    /// <summary>
    /// cache options
    /// </summary>
    public DistributedCacheEntryOptions CacheOptions => new DistributedCacheEntryOptions();
}
