using System;
using Luciano.Serafim.Ebanx.Account.Bootstrap;
using Luciano.Serafim.Ebanx.Account.Core.Abstractions.Services;
using Luciano.Serafim.Ebanx.Account.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Luciano.Serafim.Ebanx.Account.Tests.Utility;

public static class TestHelper
{
    public static IServiceCollection AddEbanxTest(this IServiceCollection services)
    {
        services
            .AddEbanxMediatR()
            .AddLogging()
            .AddEbanxDistributedCache()
            .AddEbanxResponse()
            .AddEbanxAccountService()
            .AddEbanxEventService();

        return services;
    }

    public static IServiceCollection AddEbanxAccountService(this IServiceCollection services)
    {
        services.AddScoped(_ =>
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
                    });

        return services;
    }

    public static IServiceCollection AddEbanxEventService(this IServiceCollection services)
    {
        List<Event> events = new();

        var createEvent = (Event @event) =>
        {
            var e = new Event(@event.Operation, @event.Amount, DateTime.UtcNow, @event.AccountId) { Id = Guid.NewGuid().ToString() };
            events.Add(e);

            return e;
        };

        var getEventsAfter = (int accountId, DateTime initialDate) =>  events.Where(e => e.AccountId == accountId && e.Ocurrence >= initialDate).ToList();

        services
            .AddScoped(_ =>
            {
                var service = Substitute.For<IEventService>();

                //returns a empty list of events
                service.GetEventsAfter(Arg.Any<int>(), Arg.Any<DateTime>()).Returns(call => getEventsAfter(call.ArgAt<int>(0), call.ArgAt<DateTime>(1)));
                service.CreateEvent(Arg.Any<Event>()).Returns(call => createEvent(call.ArgAt<Event>(0)));

                return service;
            });

        return services;
    }
}