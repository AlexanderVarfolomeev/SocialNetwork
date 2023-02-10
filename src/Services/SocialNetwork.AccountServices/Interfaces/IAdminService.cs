namespace SocialNetwork.AccountServices.Interfaces;

public interface IAdminService
{
    Task<bool> IsAdminAsync(Guid userId);

    Task GiveAdminRoleAsync(Guid userId, Guid accountId);
    Task RevokeAdminRoleAsync(Guid userId, Guid accountId);

    Task BanAccountAsync(Guid userId, Guid accountId);
    Task UnbanAccountAsync(Guid userId, Guid accountId);
}