using Luciano.Serafim.Ebanx.Account.Core.Abstractions.Transactions;
using Luciano.Serafim.Ebanx.Account.Core.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Luciano.Serafim.Ebanx.Account.Bootstrap.MediatR;

public class AcidBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>, IAcidEnabled where TResponse : Response
{
    private readonly ILogger<AcidBehaviour<TRequest, TResponse>> logger;
    private readonly IUnitOfWork unitOfWork;

    public AcidBehaviour(ILogger<AcidBehaviour<TRequest, TResponse>> logger, IUnitOfWork unitOfWork)
    {
        this.logger = logger;
        this.unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            await unitOfWork.StartTransactionAsync();

            var response = await next();

            if (response.IsValid)
            {
                await unitOfWork.CommitTransactionAsync();
            }
            else
            {
                await unitOfWork.RollbackTransactionAsync();
            }

            return response;
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}
