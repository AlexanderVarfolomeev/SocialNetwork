using SocialNetwork.Settings.Interfaces;
using SocialNetwork.Settings.Source;

namespace SocialNetwork.Settings.Settings;

public class AppSettings : IAppSettings
{
    private readonly IDbSettings _db = null!;
    private readonly ISettingSource _source;
    private readonly IIdentitySettings _identity;
    public AppSettings(ISettingSource source)
    {
        _source = source;
    }

    public AppSettings(IDbSettings db, ISettingSource source, IIdentitySettings identity)
    {
        _db = db;
        _source = source;
        _identity = identity;
    }
    
    public IDbSettings Db => _db ?? new DbSettings(_source);
    public IIdentitySettings Identity => _identity ?? new IdentitySettings(_source);
}