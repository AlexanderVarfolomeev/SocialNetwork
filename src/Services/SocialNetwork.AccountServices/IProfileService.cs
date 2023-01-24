using IdentityModel.Client;
using SocialNetwork.AccountServices.Models;

namespace SocialNetwork.AccountServices;

// TODO восстановление и смена пароля
// TODO изменение данных профиля
// TODO покрыть тестами

/// <summary>
/// Сервис для создания, редактирования учетной записи, выдачи токенов, подтверждения почты и тд.
/// </summary>
public interface IProfileService
{
    Task<AppAccountModel> RegisterUser(AppAccountModelRequest requestModel);
    Task<TokenResponse> LoginUser(LoginModel model);
}