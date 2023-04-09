using SocialNetwork.Settings.Interfaces;
using SocialNetwork.Settings.Source;

namespace SocialNetwork.Settings.Settings;

public class RabbitMqSettings : IRabbitMqSettings
{
    private readonly ISettingSource _source;

    public RabbitMqSettings(ISettingSource source)
    {
        _source = source;
    }

    public string Uri => _source.GetAsString("RabbitMq:Uri");
    public string UserName => _source.GetAsString("RabbitMq:UserName");
    public string Password => _source.GetAsString("RabbitMq:Password");
}