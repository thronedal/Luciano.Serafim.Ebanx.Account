using System;
using Luciano.Serafim.Ebanx.Account.Core.Exceptions;
using Luciano.Serafim.Ebanx.Account.Core.UseCases.Accounts;
using Luciano.Serafim.Ebanx.Account.Tests.Utility;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Luciano.Serafim.Ebanx.Account.Tests.Core.UseCases.Accounts;

public class CreateAccountUseCaseTests
{
    private readonly IMediator mediator;

    public CreateAccountUseCaseTests()
    {
        var services = new ServiceCollection();
        var serviceProvider = services
            .AddEbanxTest()
            .BuildServiceProvider();

        mediator = serviceProvider.GetRequiredService<IMediator>();
    }
    //# Create Account
    [Theory]
    [InlineData(100)]
    public async Task CreateAccount_Success(int accountId)
    {
        CreateAccountCommand command = new(accountId);

        var response = await mediator.Send(command);

        Assert.NotNull(response);
    }

    //# Create account - existing account
    [Theory]
    [InlineData(50)]
    public async Task CreateAccount_ExistingAccount(int accountId)
    {
        CreateAccountCommand command = new(accountId);

        await Assert.ThrowsAsync<BussinessRuleException>(async () => await mediator.Send(command));
    }
}
