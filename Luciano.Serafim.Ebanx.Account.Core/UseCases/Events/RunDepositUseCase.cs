using System;
using Luciano.Serafim.Ebanx.Account.Core.Abstractions.Services;
using Luciano.Serafim.Ebanx.Account.Core.Exceptions;
using Luciano.Serafim.Ebanx.Account.Core.Models;
using Luciano.Serafim.Ebanx.Account.Core.UseCases.Accounts;
using MediatR;

namespace Luciano.Serafim.Ebanx.Account.Core.UseCases.Events;

public class RunDepositUseCase : IRequestHandler<DepositCommand, Response<DepositResponse>>
{
    private readonly Response<DepositResponse> response;
    private readonly IAccountService accountService;
    private readonly IEventService eventService;
    private readonly IMediator mediator;

    public RunDepositUseCase(Response<DepositResponse> response, IAccountService accountService, IEventService eventService, IMediator mediator)
    {
        this.response = response;
        this.accountService = accountService;
        this.eventService = eventService;
        this.mediator = mediator;
    }
    public async Task<Response<DepositResponse>> Handle(DepositCommand request, CancellationToken cancellationToken)
    {
        //start transaction (should account be locked ??)
        try
        {
            //get destination Account
            var destination = await accountService.GetAccountById(request.DestinationId);

            //if account does not exists then create account
            if (destination is null)
            {
                destination = (await mediator.Send(new CreateAccountCommand(request.DestinationId))).GetResponseObject();
            }

            //add ammount to destination
            var deposit = (Event)request;

            //add ammount to destination
            deposit = await eventService.CreateEvent(deposit);

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

        //get destination balance
        var destinationBalance = (await mediator.Send(new GetBalanceQuery() { AccountId = request.DestinationId })).GetResponseObject();
        response.SetResponsePayload(new DepositResponse() { Destination = new AccountBalanceResponse() { Id = request.DestinationId, Balance = destinationBalance } });

        return response;
    }
}
