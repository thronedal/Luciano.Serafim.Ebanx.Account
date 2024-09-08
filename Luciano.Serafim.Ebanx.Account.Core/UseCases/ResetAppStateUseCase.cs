using System;
using System.Reflection.Metadata.Ecma335;
using Luciano.Serafim.Ebanx.Account.Core.Abstractions.Services;
using Luciano.Serafim.Ebanx.Account.Core.Models;
using MediatR;

namespace Luciano.Serafim.Ebanx.Account.Core.UseCases;

public class ResetAppStateUseCase : IRequestHandler<ResetAppStateCommand, Response<bool>>
{
    private readonly Response<bool> response;
    private readonly IAccountService accountService;
    private readonly IEventService eventService;

    public ResetAppStateUseCase(Response<bool> response, IAccountService accountService, IEventService eventService)
    {
        this.response = response;
        this.accountService = accountService;
        this.eventService = eventService;
    }

    public async Task<Response<bool>> Handle(ResetAppStateCommand request, CancellationToken cancellationToken)
    {
        await accountService.InitializeState();
        await eventService.InitializeState();

        response.SetResponsePayload(true);

        return response;
    }
}

public class ResetAppStateCommand : IRequest<Response<bool>>
{
}