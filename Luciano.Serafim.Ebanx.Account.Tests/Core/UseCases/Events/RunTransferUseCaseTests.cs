using System;
using Luciano.Serafim.Ebanx.Account.Core.Abstractions.Services;
using Luciano.Serafim.Ebanx.Account.Core.Exceptions;
using Luciano.Serafim.Ebanx.Account.Core.Models;
using Luciano.Serafim.Ebanx.Account.Core.UseCases.Events;
using Luciano.Serafim.Ebanx.Account.Tests.Utility;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Luciano.Serafim.Ebanx.Account.Tests.Core.UseCases.Events;

public class RunTransferUseCaseTests
{
    private readonly IMediator mediator;

    public RunTransferUseCaseTests()
    {
        var services = new ServiceCollection();
        var serviceProvider = services
            .AddEbanxTest()
            .BuildServiceProvider();

        mediator = serviceProvider.GetRequiredService<IMediator>();
    }

    //# Transfer from existing account
    [Theory]
    [InlineData(50, 100, 30)]
    public async Task RunTransfer_Success(int originId, int destinationId, double amount)
    {
        TransferCommand command = new(){OriginId = originId, DestinationId = destinationId, Amount = amount};

        var response = await mediator.Send(command);

        Assert.NotNull(response);
    }

    //# Transfer from non-existing account
    [Theory]
    [InlineData(5000, 100, 30)]
    public async Task RunTransfer_NonExistingOrigin(int originId, int destinationId, double amount)
    {
        TransferCommand command = new(){OriginId = originId, DestinationId = destinationId, Amount = amount};

        await Assert.ThrowsAsync<ObjectNotFoundException>(async () => await mediator.Send(command));
    }
    //# Transfer to non-existing account
    [Theory]
    [InlineData(100, 5000, 30)]
    public async Task RunTransfer_NonExistingDestination(int originId, int destinationId, double amount)
    {
        TransferCommand command = new(){OriginId = originId, DestinationId = destinationId, Amount = amount};

        await Assert.ThrowsAsync<ObjectNotFoundException>(async () => await mediator.Send(command));
    }

    //# Transfer with insuficient funds
    [Theory]
    [InlineData(5, 100, 30)]
    public async Task RunTransfer_InsuficientFunds(int originId, int destinationId, double amount)
    {
        TransferCommand command = new(){OriginId = originId, DestinationId = destinationId, Amount = amount};

        await Assert.ThrowsAsync<BussinessRuleException>(async () => await mediator.Send(command));
    }
}
