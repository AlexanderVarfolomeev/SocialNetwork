using SocialNetwork.Settings.Interfaces;
using SocialNetwork.Settings.Source;

namespace SocialNetwork.Settings.Settings;

public class AppSettings : IAppSettings
{
    private readonly IDbSettings _db;
    private readonly ISettingSource _source;
    private readonly IIdentitySettings _identity;
    private readonly IEmailSettings _email;

    public AppSettings(ISettingSource source)
    {
        _source = source;
    }

    public AppSettings(IDbSettings db, ISettingSource source, IIdentitySettings identity, IEmailSettings email)
    {
        _db = db;
        _source = source;
        _identity = identity;
        _email = email;
    }
    
    public IDbSettings Db => _db ?? new DbSettings(_source);
    public IIdentitySettings Identity => _identity ?? new IdentitySettings(_source);
    public IEmailSettings Email => _email ?? new EmailSettings(_source);
}