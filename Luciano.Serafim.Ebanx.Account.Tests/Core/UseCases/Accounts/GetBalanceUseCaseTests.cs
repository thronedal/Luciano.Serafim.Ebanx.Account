using Luciano.Serafim.Ebanx.Account.Core.Abstractions.Services;
using Luciano.Serafim.Ebanx.Account.Core.Exceptions;
using Luciano.Serafim.Ebanx.Account.Core.Models;
using Luciano.Serafim.Ebanx.Account.Core.UseCases.Accounts;
using Luciano.Serafim.Ebanx.Account.Tests.Utility;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Luciano.Serafim.Ebanx.Account.Tests.Core.UseCases.Accounts;

public class GetBalanceUseCaseTests
{
    private readonly IMediator mediator;

    public GetBalanceUseCaseTests()
    {
        var services = new ServiceCollection();
        var serviceProvider = services
            .AddEbanxTest()
            .BuildServiceProvider();

        mediator = serviceProvider.GetRequiredService<IMediator>();
    }


    //# Get balance for non-existing account
    [Fact]
    public async Task GetBalance_NonExistingAccount()
    {
        GetBalanceQuery query = new() { AccountId = 1100 };

        await Assert.ThrowsAsync<ObjectNotFoundException>(async () => await mediator.Send(query));
    }

    //# Get balance for existing account
    [Theory]
    [InlineData(50, 50)]
    [InlineData(150, 150)]
    [InlineData(250, 250)]
    public async Task GetBalance_ExistingAccount(int accountId, double balance)
    {
        GetBalanceQuery query = new() { AccountId = accountId };

        var response = await mediator.Send(query);

        Assert.NotNull(response);
        Assert.Equal(balance, response.GetResponseObject());
    }
}
