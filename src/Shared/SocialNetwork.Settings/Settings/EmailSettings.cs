using SocialNetwork.Settings.Interfaces;
using SocialNetwork.Settings.Source;

namespace SocialNetwork.Settings.Settings;

public class EmailSettings : IEmailSettings
{
    private readonly ISettingSource _source;

    public EmailSettings(ISettingSource source)
    {
        _source = source;
    }

    public string FromName => _source.GetAsString("Email:FromName");
    public string FromEmail => _source.GetAsString("Email:FromEmail");
    public string Server => _source.GetAsString("Email:Server");
    public int Port => _source.GetAsInt("Email:Port");
    public string Login => _source.GetAsString("Email:Login");
    public string Password => _source.GetAsString("Email:Password");
    public bool Ssl => _source.GetAsBool("Email:Ssl");
    public string ConfirmAddress => _source.GetAsString("Email:ConfirmAddress");
}