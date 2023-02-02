using SocialNetwork.Settings.Interfaces;
using SocialNetwork.Settings.Source;

namespace SocialNetwork.Settings.Settings;

public class IdentitySettings : IIdentitySettings
{
    private readonly ISettingSource _source;

    public IdentitySettings(ISettingSource source)
    {
        _source = source;
    }

    public string Url => _source.GetAsString("Identity:Url");
    public string ClientId => _source.GetAsString("Identity:ClientId");
    public string ClientSecret => _source.GetAsString("Identity:ClientSecret");
    public bool RequireHttps => Url.ToLower().StartsWith("https://");
}