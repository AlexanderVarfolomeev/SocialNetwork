namespace SocialNetwork.Settings.Interfaces;

public interface IRabbitMqSettings
{
    string Uri { get; }
    string UserName { get; }
    string Password { get; }
}