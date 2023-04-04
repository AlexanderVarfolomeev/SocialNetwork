using SocialNetwork.Web.Pages.Auth.Models;
using SocialNetwork.Web.Pages.Users.Models;

namespace SocialNetwork.Web.Pages.Auth.Services;

public interface IProfileService
{
    Task Register(RegisterAccountForm registerAccountModel);
    Task<AccountModel> GetProfile();
}