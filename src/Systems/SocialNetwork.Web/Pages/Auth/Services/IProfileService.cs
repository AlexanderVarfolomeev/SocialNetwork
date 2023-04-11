using SocialNetwork.Web.Pages.Auth.Models;
using SocialNetwork.Web.Pages.Profile.Model;

namespace SocialNetwork.Web.Pages.Auth.Services;

public interface IProfileService
{
    Task Register(RegisterAccountForm registerAccountModel);
    Task UpdateAccount(Guid accountId, UpdateAccount model);
}