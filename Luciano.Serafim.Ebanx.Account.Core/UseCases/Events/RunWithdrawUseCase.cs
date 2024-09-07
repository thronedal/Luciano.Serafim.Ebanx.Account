using System;
using Luciano.Serafim.Ebanx.Account.Core.Models;
using MediatR;

namespace Luciano.Serafim.Ebanx.Account.Core.UseCases.Events;

public class RunWithdrawUseCase : IRequestHandler<WithdrawCommand, Response<Event>>
{
    private readonly Response<Event> response;

    public RunWithdrawUseCase(Response<Event> response)
    {
        this.response = response;
    }

    public async Task<Response<Event>> Handle(WithdrawCommand request, CancellationToken cancellationToken)
    {
        //start transaction (should account be locked ??)
        try
        {
            //get origin account 

            //if account does not exists throws error
            
            //validate origin balance

            //remove ammount from origin

            //commit transaction (release account lock??)
        }
        catch
        {
            //rollback transaction on error
        }

        return await Task.FromResult(response);
    }
}
