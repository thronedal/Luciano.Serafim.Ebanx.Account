using System;
using Luciano.Serafim.Ebanx.Account.Core.Abstractions.Services;
using Luciano.Serafim.Ebanx.Account.Core.Exceptions;
using Luciano.Serafim.Ebanx.Account.Core.Models;
using Luciano.Serafim.Ebanx.Account.Core.UseCases.Accounts;
using MediatR;

namespace Luciano.Serafim.Ebanx.Account.Core.UseCases.Events;

public class RunWithdrawUseCase : IRequestHandler<WithdrawCommand, Response<WithdrawResponse>>
{
    private readonly Response<WithdrawResponse> response;
    private readonly IAccountService accountService;
    private readonly IEventService eventService;
    private readonly IMediator mediator;

    public RunWithdrawUseCase(Response<WithdrawResponse> response, IAccountService accountService, IEventService eventService, IMediator mediator)
    {
        this.response = response;
        this.accountService = accountService;
        this.eventService = eventService;
        this.mediator = mediator;
    }

    public async Task<Response<WithdrawResponse>> Handle(WithdrawCommand request, CancellationToken cancellationToken)
    {
        //start transaction (should account be locked ??)
        try
        {
            //get origin account 
            var origin = await accountService.GetAccountById(request.OriginId);

            //if account does not exists throws error
            if (origin is null)
            {
                throw new ObjectNotFoundException("404", request.OriginId.ToString(), nameof(request.OriginId));
            }

            //calculate balance
            var balance = (await mediator.Send(new GetBalanceQuery() { AccountId = origin.Id })).GetResponseObject();

            //validate origin balance
            if (balance < request.Amount)
            {
                throw new BussinessRuleException($"Balance '{balance:C2}' should be higher than the operation amount $'{request.Amount:C2}'");
            }

            var withdraw = (Event)request;

            //remove ammount from origin
            withdraw = await eventService.CreateEvent(withdraw);

            //commit transaction 
        }
        catch
        {
            //rollback transaction on error
            throw;
        }
        finally
        {
            //(release account lock??)
        }

        //get origin balance
        var originBalance = (await mediator.Send(new GetBalanceQuery() { AccountId = request.OriginId })).GetResponseObject();
        response.SetResponsePayload(new WithdrawResponse(){ Origin = new AccountBalanceResponse() { Id = request.OriginId, Balance = originBalance}});

        return await Task.FromResult(response);
    }
}
