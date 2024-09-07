using System;
using Luciano.Serafim.Ebanx.Account.Core.Abstractions.Services;
using Luciano.Serafim.Ebanx.Account.Core.Models;
using MediatR;

namespace Luciano.Serafim.Ebanx.Account.Core.UseCases.Accounts;

public class GetBalanceUseCase : IRequestHandler<GetBalanceQuery, Response<double>>
{
    private readonly Response<double> response;
    private readonly IAccountService accountService;
    private readonly IEventService eventService;

    public GetBalanceUseCase(Response<double> response, IAccountService accountService, IEventService eventService)
    {
        this.response = response;
        this.accountService = accountService;
        this.eventService = eventService;
    }

    public async Task<Response<double>> Handle(GetBalanceQuery request, CancellationToken cancellationToken)
    {   
        var debitOperations = new []{EventOperation.OutgoingTransfer, EventOperation.Withdraw};

        //get account
        var account = await accountService.GetAccountById(request.AccountId);

        //account exists?
        if(account is null)
        {
            throw new Exception("Account does not exists");
        }

        //get balance        
        var consolidatedBalance =  await accountService.GetLastConsolidatedBalance(request.AccountId);
        var events = await eventService.GetEvetsAfter(account.Id, consolidatedBalance.BalanceDate.ToDateTime(new TimeOnly()));

        //calculate balance
        var balance = consolidatedBalance.Balance + events.Sum(e => e.Amount * (debitOperations.Contains(e.Operation) ? -1 : 1));

        response.SetResponsePayload(balance);

        return response;
    }
}
