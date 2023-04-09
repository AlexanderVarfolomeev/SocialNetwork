using SocialNetwork.Settings.Interfaces;
using SocialNetwork.Settings.Source;

namespace SocialNetwork.Settings.Settings;

public class RedisSettings : IRedisSettings
{ 
    private readonly ISettingSource _source;

    public RedisSettings(ISettingSource source)
    {
        _source = source;
    }

    public string Uri => _source.GetAsString("Cache:Uri");
    public int CacheLifeTime => _source.GetAsInt("Cache:CacheLifeTime");
}