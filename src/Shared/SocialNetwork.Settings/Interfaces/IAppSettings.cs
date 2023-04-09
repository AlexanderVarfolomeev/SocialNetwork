namespace SocialNetwork.Settings.Interfaces;

public interface IAppSettings
{
    IDbSettings Db { get; }
    IIdentitySettings Identity { get; }
    IEmailSettings Email { get; }
    IRedisSettings Redis { get; }
    IRabbitMqSettings Rabbit { get; }
}