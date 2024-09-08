using System;
using Luciano.Serafim.Ebanx.Account.Core.Abstractions.Services;
using Luciano.Serafim.Ebanx.Account.Core.Exceptions;
using Luciano.Serafim.Ebanx.Account.Core.Helpers;
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
        //get account
        var account = await accountService.GetAccountById(request.AccountId);

        //account exists?
        if(account is null)
        {
            throw new ObjectNotFoundException("404", request.AccountId.ToString(), nameof(request.AccountId));
        }

        //get balance        
        var consolidatedBalance =  await accountService.GetLastConsolidatedBalance(request.AccountId);
        var events = await eventService.GetEventsAfter(account.Id, consolidatedBalance.BalanceDate.ToDateTime(new TimeOnly()) );

        //calculate balance
        var balance = BalanceHelper.CalculateBalance(consolidatedBalance.Balance, events);

        response.SetResponsePayload(balance);

        return response;
    }
}
