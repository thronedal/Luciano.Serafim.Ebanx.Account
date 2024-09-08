using System;
using Luciano.Serafim.Ebanx.Account.Core.Abstractions.Services;
using Luciano.Serafim.Ebanx.Account.Core.Exceptions;
using Luciano.Serafim.Ebanx.Account.Core.Models;
using Luciano.Serafim.Ebanx.Account.Core.UseCases.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Luciano.Serafim.Ebanx.Account.Tests.Core.UseCases.Events;

public class RunTransferUseCaseTests
{
    private readonly IMediator mediator;

    public RunTransferUseCaseTests()
    {
        var createEvent = (Event @event) => {
            return new Event(@event.Operation, @event.Amount, DateTime.UtcNow, @event.AccountId){Id = Guid.NewGuid().ToString()};
        };

        var services = new ServiceCollection();
        var serviceProvider = services
            .AddLogging()
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(TransferCommand).Assembly))
            .AddScoped(_ =>
            {
                var service = Substitute.For<IAccountService>();

                //account exists if id < 1000
                service.GetAccountById(Arg.Is<int>(a => a < 1000)).Returns(call =>
                    new Account.Core.Models.Account(call.ArgAt<int>(0), call.ArgAt<int>(0).ToString())
                    );
                //account does not exists if id >= 1000
                service.GetAccountById(Arg.Is<int>(a => a >= 1000)).Returns(null as Account.Core.Models.Account);

                //return the last consolidated balance (set the date for today, and the balance to the account id)
                service.GetLastConsolidatedBalance(Arg.Any<int>()).Returns(call =>
                    new AccountConsolidatedBalance(
                        new Account.Core.Models.Account(call.ArgAt<int>(0), call.ArgAt<int>(0).ToString()),
                        DateOnly.FromDateTime(DateTime.UtcNow),
                        call.ArgAt<int>(0)
                        )
                    );

                return service;
            })            
            .AddScoped(_ =>
            {
                var service = Substitute.For<IEventService>();

                //returns a empty list of events
                service.GetEvetsAfter(Arg.Any<int>(), Arg.Any<DateTime>()).Returns(Enumerable.Empty<Event>());
                service.CreateEvent(Arg.Any<Event>()).Returns(call => createEvent(call.ArgAt<Event>(0)));

                return service;
            })
            .AddScoped(typeof(Response<>), typeof(Response<>))            
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
