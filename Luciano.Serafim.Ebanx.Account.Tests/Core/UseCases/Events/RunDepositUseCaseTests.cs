using System;
using Luciano.Serafim.Ebanx.Account.Core.UseCases.Events;
using Luciano.Serafim.Ebanx.Account.Tests.Utility;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Luciano.Serafim.Ebanx.Account.Tests.Core.UseCases.Events;

public class RunDepositUseCaseTests
{
    private readonly IMediator mediator;

    public RunDepositUseCaseTests()
    {
        var services = new ServiceCollection();
        var serviceProvider = services
            .AddEbanxTest()
            .BuildServiceProvider();

        mediator = serviceProvider.GetRequiredService<IMediator>();
    }

    //# Create account with initial balance
    [Theory]
    [InlineData(100, 10)]
    public async Task RunDeposit_CreateAccount(int destinationId, double amount)
    {
        DepositCommand command = new(destinationId, amount);

        var response = await mediator.Send(command);

        Assert.NotNull(response);
    }

    //# Deposit into existing account
    [Theory]
    [InlineData(50, 10)]
    public async Task RunDeposit_ExistingAccount(int destinationId, double amount)
    {
        DepositCommand command = new(destinationId, amount);

        var response = await mediator.Send(command);

        Assert.NotNull(response);
    }

}
