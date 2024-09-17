using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;


namespace Luciano.Serafim.Ebanx.Account.Bootstrap.MediatR;

/// <summary>
/// behaviour that enable log before and after a MediatR pipeline execution
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> logger;

    /// <inheritdoc/>
    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        this.logger = logger;
    }

    /// <inheritdoc/>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        string requestType = typeof(TRequest).Name;
        logger.LogInformation("Handling '{type}'", requestType);
        logger.LogDebug(message: "Handle '{type}' request: {command}", requestType, JsonSerializer.Serialize(request));
        var response = await next();
        logger.LogInformation("Handled {type}", requestType);
        logger.LogDebug(message: "Handle '{type}' request: {command}", response?.GetType().Name, JsonSerializer.Serialize(response));

        return response;
    }
}