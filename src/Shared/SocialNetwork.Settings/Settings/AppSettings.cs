using SocialNetwork.Settings.Interfaces;
using SocialNetwork.Settings.Source;

namespace SocialNetwork.Settings.Settings;

public class AppSettings : IAppSettings
{
    private readonly IDbSettings _db = null!;
    private readonly ISettingSource _source;

    public AppSettings(ISettingSource source)
    {
        _source = source;
    }

    public AppSettings(IDbSettings db, ISettingSource source)
    {
        _db = db;
        _source = source;
    }
    
    public IDbSettings Db => _db ?? new DbSettings(_source);
}