using SocialNetwork.Web.Pages.Auth.Models;

namespace SocialNetwork.Web.Pages.Auth.Services;

public interface IAuthService
{
    Task<LoginResult> Login(LoginModel loginModel);
    Task Logout();
}