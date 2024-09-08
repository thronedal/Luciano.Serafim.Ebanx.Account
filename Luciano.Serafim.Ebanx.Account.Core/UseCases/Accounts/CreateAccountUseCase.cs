using System;
using Luciano.Serafim.Ebanx.Account.Core.Abstractions.Services;
using Luciano.Serafim.Ebanx.Account.Core.Exceptions;
using Luciano.Serafim.Ebanx.Account.Core.Models;
using MediatR;

namespace Luciano.Serafim.Ebanx.Account.Core.UseCases.Accounts;

public class CreateAccountUseCase : IRequestHandler<CreateAccountCommand, Response<Models.Account>>
{
    private readonly Response<Models.Account> response;
    private readonly IAccountService accountService;

    public CreateAccountUseCase(Response<Models.Account> response, IAccountService accountService)
    {
        this.response = response;
        this.accountService = accountService;
    }
    public async Task<Response<Models.Account>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        //begin transaction
        try
        {
            //check if account exists
            bool exists = await accountService.AccountExists(request.AccountId);

            if (exists)
            { 
                throw new BussinessRuleException($"Account Id '{request.AccountId}' already exist and can not be created");
            }

            //creates account
            Models.Account account = (Models.Account)request;
            account = await accountService.CreateAccount(account);

            //consolidate initial balance (0)
            AccountConsolidatedBalance consolidatedBalance = await accountService.ConsolidateBalance(account, DateOnly.FromDateTime(DateTime.UtcNow), 0.0);

            //commits transaction

            response.SetResponsePayload(account);
        }
        catch
        {
            //rollback transaction
            throw;
        }
        return response;
    }
}
