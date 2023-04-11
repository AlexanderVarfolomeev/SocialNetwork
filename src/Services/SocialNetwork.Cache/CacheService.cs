using Serilog;
using Serilog.Core;
using SocialNetwork.Common.Extensions;
using SocialNetwork.Settings.Interfaces;
using StackExchange.Redis;

namespace SocialNetwork.Cache;

public class CacheService : ICacheService
{
    private readonly TimeSpan _defaultLifetime;

    private readonly IDatabase _cacheDb;

    private static string _redisUri;
    private static ConnectionMultiplexer Connection => _lazyConnection.Value;
    private static Lazy<ConnectionMultiplexer> _lazyConnection = new(() => ConnectionMultiplexer.Connect(_redisUri));

    public CacheService(IAppSettings settings)
    {
        _redisUri = settings.Redis.Uri;
        _defaultLifetime = TimeSpan.FromMinutes(settings.Redis.CacheLifeTime);

        _cacheDb = Connection.GetDatabase();
    }

    public string KeyGenerate()
    {
        return Guid.NewGuid().Shrink();
    }

    public async Task<bool> Delete(string key)
    {
        try
        {
            return await _cacheDb.KeyDeleteAsync(key);
        }
        catch (Exception ex)
        {
            throw new Exception($"Can`t delete data from cache for {key}", ex);
        }
        
    }

    public async Task<T> Get<T>(string key, bool resetLifeTime = false)
    {
        try
        {
            string cachedData = (await _cacheDb.StringGetAsync(key))!;
            if (cachedData.IsNullOrEmpty())
                return default;

            var data = cachedData.FromJsonString<T>();

            if (resetLifeTime)
                await SetStoreTime(key);

            return data;
        }
        catch (Exception ex)
        {
            throw new Exception($"Can`t get data from cache for {key}", ex);
        }
    }

    public async Task<bool> Put<T>(string key, T data, TimeSpan? storeTime = null)
    {
        try
        {
            return await _cacheDb.StringSetAsync(key, data.ToJsonString(), storeTime ?? _defaultLifetime);

        }
        catch (Exception ex)
        {
            throw new Exception($"Can't put cache {key}", ex);
        }
    }

    public async Task SetStoreTime(string key, TimeSpan? storeTime = null)
    {
        await _cacheDb.KeyExpireAsync(key, storeTime ?? _defaultLifetime);
    }
}