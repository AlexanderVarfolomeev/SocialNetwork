namespace SocialNetwork.Settings.Interfaces;

public interface IDbSettings
{
    string GetConnectionString { get; }
}