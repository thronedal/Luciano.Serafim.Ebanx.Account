using Luciano.Serafim.Ebanx.Account.Core.Abstractions.Locking;
using Luciano.Serafim.Ebanx.Account.Core.Abstractions.Transactions;
using Luciano.Serafim.Ebanx.Account.Core.Exceptions;
using Luciano.Serafim.Ebanx.Account.Core.Models;
using Medallion.Threading;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Luciano.Serafim.Ebanx.Account.Bootstrap.MediatR;

public class LockingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>, IResourceLocking where TResponse : Response
{
    private readonly ILogger<LockingBehaviour<TRequest, TResponse>> logger;
    private readonly IDistributedLockProvider synchronizationProvider;

    public LockingBehaviour(ILogger<LockingBehaviour<TRequest, TResponse>> logger, IDistributedLockProvider synchronizationProvider)
    {
        this.logger = logger;
        this.synchronizationProvider = synchronizationProvider;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var locks = new List<IDistributedSynchronizationHandle>();
        try
        {
            //lock/wait
            foreach (var resourceName in request.Resources.Split(","))
            {
                locks.Add(await synchronizationProvider.AcquireLockAsync(resourceName, request.TimeOut));
            }

            var response = await next();
            return response;
        }
        catch (TimeoutException ex)
        {
            throw new ResourceLockingTimeOutException($"Resource locking failed for resources: {request.Resources}", ex);
        }
        finally
        {
            //release lock
            foreach (var l in locks)
            {
                l.Dispose();
            }
        }
    }
}