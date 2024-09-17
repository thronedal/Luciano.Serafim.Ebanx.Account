using Luciano.Serafim.Ebanx.Account.Core.Abstractions.Caching;
using Luciano.Serafim.Ebanx.Account.Core.Models;
using Luciano.Serafim.Ebanx.Account.Core.Utils;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Luciano.Serafim.Ebanx.Account.Bootstrap.MediatR;

/// <summary>
/// A behaviour that utilizes cache to bypass the handler execution, or save the results on the cache for future use
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
/// <inheritdoc/>
public class CachingBehaviour<TRequest, TResponse>(ILogger<CachingBehaviour<TRequest, TResponse>> logger, IDistributedCache cache, TResponse response) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>, ICacheable where TResponse : Response
{
    private readonly ILogger<CachingBehaviour<TRequest, TResponse>> logger = logger;
    private readonly IDistributedCache cache = cache;
    private readonly TResponse response = response;

    /// <inheritdoc/>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        Type reponseType = typeof(object);
        string requestType = typeof(TRequest).Name;

        var iReq = typeof(TRequest).GetInterfaces().Where(x =>
                x.IsGenericType &&
                x.GetGenericTypeDefinition() == typeof(IRequest<>)).FirstOrDefault();

        if (iReq is not null)
        {
            reponseType = iReq.GenericTypeArguments[0].GenericTypeArguments[0];
        }

        TResponse cachedResponse;

        logger.LogInformation("Getting cache for '{type}', Key '{key}'", requestType, request.CacheKey);
        var cachedValue = await cache.GetValueAsync(reponseType, request.CacheKey);

        if (cachedValue == null)
        {
            logger.LogInformation("No cache found for '{type}', Key '{key}'", requestType, request.CacheKey);
            cachedResponse = await next();

            if (cachedResponse.IsValid)
            {
                logger.LogInformation("Setting cache for '{type}', Key '{key}'", requestType, request.CacheKey);
                await cache.SetAsync(request.CacheKey, cachedResponse.GetResponseObject(reponseType), request.CacheOptions);
                logger.LogDebug(message: "Cache set for Key '{key}', response: {response}", request.CacheKey, JsonSerializer.Serialize(cachedResponse));
                response.SetResponsePayload(cachedResponse.GetResponseObject(reponseType));
            }
            else
            {
                logger.LogDebug(message: "Invalid Response!\nNo cache set for Key '{key}', response: {response}", request.CacheKey, JsonSerializer.Serialize(cachedResponse));
                response.Status = cachedResponse.Status;
                response.Errors.AddRange(cachedResponse.Errors);
            }

            return response;
        }
        else
        {
            logger.LogInformation("Cache hit for '{type}', Key '{key}'", requestType, request.CacheKey);

            response.SetResponsePayload(cachedValue);

            logger.LogDebug(message: "Cache found for Key '{key}' response: {response}", request.CacheKey, JsonSerializer.Serialize(response));
        }

        return response;
    }
}

