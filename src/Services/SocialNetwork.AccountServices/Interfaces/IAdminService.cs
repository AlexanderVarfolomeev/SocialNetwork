namespace SocialNetwork.AccountServices.Interfaces;

public interface IAdminService
{
    Task<bool> IsAdminAsync(Guid userId);

    Task GiveAdminRoleAsync(Guid userId);
    Task RevokeAdminRoleAsync(Guid userId);

    Task BanUserAsync(Guid userId);
    Task UnbanUserAsync(Guid userId);
}