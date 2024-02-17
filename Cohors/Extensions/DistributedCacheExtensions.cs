using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Cohors.Extensions;

/// <summary>
/// Provides extension methods for the IDistributedCache interface.
/// </summary>
public static class DistributedCacheExtensions
{
    /// <summary>
    /// Asynchronously sets a record in the cache.
    /// </summary>
    /// <param name="cache">The distributed cache where the record will be set.</param>
    /// <param name="recordId">The unique identifier for the record.</param>
    /// <param name="data">The data to be stored in the cache.</param>
    /// <param name="absoluteExpireTime">The absolute expiration time for the record. If null, defaults to 60 seconds.</param>
    /// <param name="unusedExpireTime">The sliding expiration time for the record. If the record is not accessed within this time, it will be removed.</param>
    /// <typeparam name="T">The type of the data to be stored in the cache.</typeparam>
    public static async Task SetRecordAsync<T>(
        this IDistributedCache cache,
        string recordId,
        T data,
        TimeSpan? absoluteExpireTime = null,
        TimeSpan? unusedExpireTime = null)
    {
        var options = new DistributedCacheEntryOptions();
        options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60);
        options.SlidingExpiration = unusedExpireTime;

        var jsonData = JsonSerializer.Serialize(data);
        await cache.SetStringAsync(recordId, jsonData, options);
    }

    /// <summary>
    /// Asynchronously retrieves a record from the cache.
    /// </summary>
    /// <param name="cache">The distributed cache from which the record will be retrieved.</param>
    /// <param name="recordId">The unique identifier for the record.</param>
    /// <typeparam name="T">The type of the data to be retrieved from the cache.</typeparam>
    /// <returns>The data retrieved from the cache, or the default value for type T if the record does not exist.</returns>
    public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
    {
        var jsonData = await cache.GetStringAsync(recordId);
        if (jsonData is null)
        {
            return default(T);
        }
        return JsonSerializer.Deserialize<T>(jsonData);
    }
}
