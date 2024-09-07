using System;
using Luciano.Serafim.Ebanx.Account.Core.Models;
using MediatR;

namespace Luciano.Serafim.Ebanx.Account.Core.UseCases.Events;

public class RunDepositUseCase : IRequestHandler<DepositCommand, Response<Event>>
{
    private readonly Response<Event> response;

    public RunDepositUseCase(Response<Event> response)
    {
        this.response = response;
    }
    public async Task<Response<Event>> Handle(DepositCommand request, CancellationToken cancellationToken)
    {
        //start transaction (should account be locked ??)
        try
        {
            //get destination Account

            //if account does not exists then create account

            //add ammount to destination

            //commit transaction (release account lock??)
        }
        catch
        {
            //rollback transaction on error
        }

        return await Task.FromResult(response);
    }
}
