using SocialNetwork.Settings.Interfaces;
using SocialNetwork.Settings.Source;

namespace SocialNetwork.Settings.Settings;

public class DbSettings : IDbSettings
{
    private readonly ISettingSource _source;

    public DbSettings(ISettingSource source)
    {
        _source = source;
    }

    public string ConnectionString => _source.GetAsString("Database:ConnectionString");
}