using SocialNetwork.Web.Pages.Users.Models;

namespace SocialNetwork.Web.Pages.Users.Services;

public interface IAccountService
{
    Task<IEnumerable<AccountModel>> GetAccounts(int offset = 0, int limit = 100);
    Task<AccountModel> GetAccount(Guid accountId);
}