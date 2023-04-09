using SocialNetwork.Settings.Interfaces;
using SocialNetwork.Settings.Source;

namespace SocialNetwork.Settings.Settings;

public class AppSettings : IAppSettings
{
    private readonly IDbSettings _db;
    private readonly ISettingSource _source;
    private readonly IIdentitySettings _identity;
    private readonly IEmailSettings _email;
    private readonly IRedisSettings _redis;

    public AppSettings(ISettingSource source)
    {
        _source = source;
    }

    public AppSettings(IDbSettings db, ISettingSource source, IIdentitySettings identity, IEmailSettings email, IRedisSettings redis)
    {
        _db = db;
        _source = source;
        _identity = identity;
        _email = email;
        _redis = redis;
    }
    
    public IDbSettings Db => _db ?? new DbSettings(_source);
    public IIdentitySettings Identity => _identity ?? new IdentitySettings(_source);
    public IEmailSettings Email => _email ?? new EmailSettings(_source);

    public IRedisSettings Redis => _redis ?? new RedisSettings(_source);
}