
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Luciano.Serafim.Ebanx.Account.Tests.Utility;
using Luciano.Serafim.Ebanx.Account.Core.Models;
using Luciano.Serafim.Ebanx.Account.Core.Utils;
using Luciano.Serafim.Ebanx.Account.Core.Abstractions.Caching;

namespace Luciano.Serafim.Ebanx.Account.Tests.Core.MediatR;

public class CachingInvalidationBehaviourTests
{
    private readonly IMediator mediator;
    private readonly IDistributedCache cache;

    public CachingInvalidationBehaviourTests()
    {
        var services = new ServiceCollection();
        var serviceProvider = services
            .AddEbanxTest()
            .BuildServiceProvider();

        mediator = serviceProvider.GetRequiredService<IMediator>();
        cache = serviceProvider.GetRequiredService<IDistributedCache>();
    }

    [Fact]
    public async Task InvalidateCache_Success()
    {
        await cache.SetAsync("cache_invalidate_test", JsonSerializer.Serialize(10));

        await mediator.Send(new GetValueCommandTest());

        var bytes = await cache.GetAsync("cache_invalidate_test");

        Assert.Null(bytes);
    }
}

/// <summary>
/// Test command for invalidating caching test
/// To makes a command utilize the behaviour the ICacheInvalidation must be implemented on the query/command
/// </summary>
public class GetValueCommandTest : IRequest<Response<int>>, ICacheInvalidation
{
    public int Value { get; set; }

    public IEnumerable<string> KeysToInvalidate => new[] { "cache_invalidate_test" };
}

public class InvalidateCacheBehaviourTestUseCase : IRequestHandler<GetValueCommandTest, Response<int>>
{
    private readonly Response<int> response;

    public InvalidateCacheBehaviourTestUseCase(Response<int> response)
    {
        this.response = response;
    }
    public async Task<Response<int>> Handle(GetValueCommandTest request, CancellationToken cancellationToken)
    {
        response.SetResponsePayload(request.Value);
        return await Task.FromResult(response);
    }
}
