using SocialNetwork.AccountServices.Models;

namespace SocialNetwork.AccountServices.Interfaces;

/// <summary>
/// CRUD сервис для работы с аккаунтами.
/// </summary>
public interface IAccountService
{
    Task<AppAccountModel> GetAccountAsync(Guid id);
    Task<AppAccountModel> GetAccountAsync(string username);
    Task<IEnumerable<AppAccountModel>> GetAccountsAsync(int offset = 0, int limit = 10);
    Task<AppAccountModel> UpdateAccountAsync(Guid id, AppAccountUpdateModel model);
    Task DeleteAccountAsync(Guid id);
}