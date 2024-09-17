using Luciano.Serafim.Ebanx.Account.Core.Abstractions.Caching;
using Luciano.Serafim.Ebanx.Account.Core.Models;
using Luciano.Serafim.Ebanx.Account.Core.Utils;
using Luciano.Serafim.Ebanx.Account.Tests.Utility;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace Luciano.Serafim.Ebanx.Account.Tests.Core.MediatR;

public class CachingBehaviourTests
{
    private readonly IMediator mediator;
    private readonly IDistributedCache cache;

    public CachingBehaviourTests()
    {
        var services = new ServiceCollection();
        var serviceProvider = services
            .AddEbanxTest()
            .BuildServiceProvider();

        mediator = serviceProvider.GetRequiredService<IMediator>();
        cache = serviceProvider.GetRequiredService<IDistributedCache>();
    }

    [Fact]
    public async Task CreateCache_Success()
    {
        var result = await mediator.Send(new GetValueQueryTest(){Value=15});

        var cachedResponse = await cache.GetValueAsync<int>("cache_hit_test");

        Assert.Equal(15, result.GetResponseObject());
        Assert.Equal(15, cachedResponse);
    }   

    [Fact]     
    public async Task CacheHit_Success()
    {
        await cache.SetAsync("cache_hit_test", 10);

        var result = await mediator.Send(new GetValueQueryTest());

        Assert.Equal(10, result.GetResponseObject());
    }
}

/// <summary>
/// Test Query utilizinf the query behaviour
/// To utilize cache its only need do implements ICacheable Interface
/// </summary>
public class GetValueQueryTest : IRequest<Response<int>>, ICacheable
{
    public string CacheKey => "cache_hit_test";

    public int Value { get; set; }
}

public class CachingBehaviourTestUseCase : IRequestHandler<GetValueQueryTest, Response<int>>
{
    private readonly Response<int> response;

    public CachingBehaviourTestUseCase(Response<int> response)
    {
        this.response = response;
    }
    public async Task<Response<int>> Handle(GetValueQueryTest request, CancellationToken cancellationToken)
    {
        response.SetResponsePayload(request.Value);
        return await Task.FromResult(response);
    }
}