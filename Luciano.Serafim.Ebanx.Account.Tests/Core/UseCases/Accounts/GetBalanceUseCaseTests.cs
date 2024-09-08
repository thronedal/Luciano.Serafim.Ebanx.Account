using Luciano.Serafim.Ebanx.Account.Core.Abstractions.Services;
using Luciano.Serafim.Ebanx.Account.Core.Exceptions;
using Luciano.Serafim.Ebanx.Account.Core.Models;
using Luciano.Serafim.Ebanx.Account.Core.UseCases.Accounts;
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
            .AddLogging()
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetBalanceQuery).Assembly))
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

                return service;
            })
            .AddScoped(typeof(Response<>), typeof(Response<>))            
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
