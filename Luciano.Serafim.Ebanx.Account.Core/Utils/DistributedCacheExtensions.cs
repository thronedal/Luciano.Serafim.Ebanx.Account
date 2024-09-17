using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace Luciano.Serafim.Ebanx.Account.Core.Utils;

/// <summary>
/// Helper class to manage cache
/// </summary>
public static class DistributedCacheExtensions
{
    /// <summary>
    /// Set a new value in cache
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cache"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Task SetAsync<T>(this IDistributedCache cache, string key, T value)
    {
        return cache.SetAsync(key, value, new DistributedCacheEntryOptions());
    }

    /// <summary>
    /// Set a new value in cache
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cache"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static async Task SetAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options)
    {
        var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value));
        await cache.SetAsync(key, bytes, options);
    }

    /// <summary>
    /// Get a value from cache or initiate the cache with the function result
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cache"></param>
    /// <param name="key"></param>
    /// <param name="setValue"></param>
    /// <returns></returns>
    public static async Task<T> GetOrSetValueAsync<T>(this IDistributedCache cache, string key, Func<Task<T>> setValue)
    {
        T? value;
        var bytes = await cache.GetAsync(key);

        if (bytes == null)
        {
            value = await setValue();
            await cache.SetAsync(key, value);
        }
        else
        {
            value = JsonSerializer.Deserialize<T>(bytes);
        }

        return value!;
    }

    /// <summary>
    /// Get a value from cache'
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cache"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static async Task<T?> GetValueAsync<T>(this IDistributedCache cache, string key)
    {
        return (T?)await cache.GetValueAsync(typeof(T), key);
    }

    /// <summary>
    /// Get a value from cache'
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cache"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static async Task<object?> GetValueAsync(this IDistributedCache cache, Type type, string key)
    {
        object? value;
        var bytes = await cache.GetAsync(key);

        if (bytes != null)
        {
            value = JsonSerializer.Deserialize(bytes, type);
        }
        else
        {
            value = default;
        }

        return await Task.FromResult(value);
    }
}