using System;
using Luciano.Serafim.Ebanx.Account.Core.Exceptions;
using Luciano.Serafim.Ebanx.Account.Core.UseCases.Events;
using Luciano.Serafim.Ebanx.Account.Tests.Utility;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Luciano.Serafim.Ebanx.Account.Tests.Core.UseCases.Events;

public class RunWithdrawUseCaseTests
{
    private readonly IMediator mediator;

    public RunWithdrawUseCaseTests()
    {
        var services = new ServiceCollection();
        var serviceProvider = services
            .AddEbanxTest()
            .BuildServiceProvider();

        mediator = serviceProvider.GetRequiredService<IMediator>();
    }

    //# Withdraw from existing account
    [Theory]
    [InlineData(50, 30)]
    public async Task RunWithdraw_Success(int originId, double amount)
    {
        WithdrawCommand command = new() { OriginId = originId, Amount = amount };

        var response = await mediator.Send(command);

        Assert.NotNull(response);
    }

    //# Withdraw from non-existing account
    [Theory]
    [InlineData(5000, 30)]
    public async Task RunWithdraw_NonExistingOrigin(int originId, double amount)
    {
        WithdrawCommand command = new() { OriginId = originId, Amount = amount };

        await Assert.ThrowsAsync<ObjectNotFoundException>(async () => await mediator.Send(command));
    }
    
    //# Withdraw with insuficient funds
    [Theory]
    [InlineData(5, 30)]
    public async Task RunWithdraw_InsuficientFunds(int originId, double amount)
    {
        WithdrawCommand command = new() { OriginId = originId, Amount = amount };

        await Assert.ThrowsAsync<BussinessRuleException>(async () => await mediator.Send(command));
    }
}
