using System.Linq.Expressions;
using SocialNetwork.AccountServices.Models;

namespace SocialNetwork.AccountServices;

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
    Task BanUserAsync(Guid id);
    Task<bool> IsUserBanned(Guid id);
    Task<bool> IsAdmin(Guid id);
}