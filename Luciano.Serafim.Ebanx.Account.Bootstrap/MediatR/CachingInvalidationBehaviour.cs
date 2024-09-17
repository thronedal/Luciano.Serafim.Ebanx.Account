using Luciano.Serafim.Ebanx.Account.Core.Abstractions.Caching;
using Luciano.Serafim.Ebanx.Account.Core.Models;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Luciano.Serafim.Ebanx.Account.Bootstrap.MediatR;


/// <summary>
/// behaviour tha invalidates a cache entry
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class CachingInvalidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>, ICacheInvalidation where TResponse : Response
{
    private readonly ILogger<CachingInvalidationBehaviour<TRequest, TResponse>> logger;
    private readonly IDistributedCache cache;

    /// <inheritdoc/>
    public CachingInvalidationBehaviour(ILogger<CachingInvalidationBehaviour<TRequest, TResponse>> logger, IDistributedCache cache)
    {
        this.logger = logger;
        this.cache = cache;
    }

    /// <inheritdoc/>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        string requestType = typeof(TRequest).Name;

        TResponse response;

        response = await next();

        if (response.IsValid)
        {
            foreach (var cacheKey in request.KeysToInvalidate)
            {
                await cache.RemoveAsync(cacheKey, cancellationToken);

                logger.LogInformation("Cache Key '{key}' invalidated by command '{type}'", cacheKey, requestType);
            }
        }

        return response;
    }
}
