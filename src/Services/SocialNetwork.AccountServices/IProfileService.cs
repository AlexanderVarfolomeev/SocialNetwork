using IdentityModel.Client;
using SocialNetwork.AccountServices.Models;

namespace SocialNetwork.AccountServices;

// TODO восстановление и смена пароля
// TODO изменение данных профиля
// TODO покрыть тестами
// TODO logout


/// <summary>
/// Сервис для создания, редактирования учетной записи, выдачи токенов, подтверждения почты и тд.
/// </summary>
public interface IProfileService
{
    Task<AppAccountModel> RegisterUserAsync(AppAccountModelRequest requestModel);
    Task<TokenResponse> LoginUserAsync(LoginModel model);
}