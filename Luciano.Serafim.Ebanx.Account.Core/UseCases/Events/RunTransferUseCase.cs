using System.Data.Common;
using Luciano.Serafim.Ebanx.Account.Core.Abstractions.Services;
using Luciano.Serafim.Ebanx.Account.Core.Exceptions;
using Luciano.Serafim.Ebanx.Account.Core.Helpers;
using Luciano.Serafim.Ebanx.Account.Core.Models;
using Luciano.Serafim.Ebanx.Account.Core.UseCases.Accounts;
using MediatR;

namespace Luciano.Serafim.Ebanx.Account.Core.UseCases.Events;

public class RunTransferUseCase : IRequestHandler<TransferCommand, Response<TransferResponse>>
{
    private readonly Response<TransferResponse> response;
    private readonly IAccountService accountService;
    private readonly IEventService eventService;
    private readonly IMediator mediator;

    public RunTransferUseCase(Response<TransferResponse> response, IAccountService accountService, IEventService eventService, IMediator mediator)
    {
        this.response = response;
        this.accountService = accountService;
        this.eventService = eventService;
        this.mediator = mediator;
    }

    public async Task<Response<TransferResponse>> Handle(TransferCommand request, CancellationToken cancellationToken)
    {
        //get origin account 
        var origin = await accountService.GetAccountById(request.OriginId);

        //if account does not exists throws error
        if (origin is null)
        {
            throw new ObjectNotFoundException("404", request.OriginId.ToString(), nameof(request.OriginId));
        }

        //get destination Account
        var destination = await accountService.GetAccountById(request.DestinationId);

        //if account does not exists throws error
        if (destination is null)
        {
            throw new ObjectNotFoundException("404", request.DestinationId.ToString(), nameof(request.DestinationId));
        }

        //calculate balance
        var balance = (await mediator.Send(new GetBalanceQuery(origin.Id))).GetResponseObject();

        //validate origin balance
        if (balance < request.Amount)
        {
            throw new BussinessRuleException($"Balance '{balance:C2}' should be higher than the operation amount '${request.Amount:C2}'");
        }

        var transfeEvents = (Event[])request;
        var outgoingTransfer = transfeEvents[0];
        var incommingTransfer = transfeEvents[1];

        //remove ammount from origin
        outgoingTransfer = await eventService.CreateEvent(outgoingTransfer);

        //add ammount to destination
        incommingTransfer.OriginEventId = outgoingTransfer.Id;
        incommingTransfer = await eventService.CreateEvent(incommingTransfer);



        //get origin balance
        var originBalance = (await mediator.Send(new GetBalanceQuery(request.OriginId))).GetResponseObject();

        //get destination balance
        var destinationBalance = (await mediator.Send(new GetBalanceQuery(request.DestinationId))).GetResponseObject();

        TransferResponse dto = new(new(request.OriginId, originBalance), new(request.DestinationId, destinationBalance));

        response.SetResponsePayload(dto);

        return response;
    }
}
