using AutoMapper;
using IdentityModel.Client;
using Microsoft.AspNetCore.Identity;
using SocialNetwork.AccountServices.Interfaces;
using SocialNetwork.AccountServices.Models;
using SocialNetwork.Common.Enum;
using SocialNetwork.Common.Exceptions;
using SocialNetwork.Common.Extensions;
using SocialNetwork.Constants.Email;
using SocialNetwork.Constants.Errors;
using SocialNetwork.Constants.Security;
using SocialNetwork.EmailService;
using SocialNetwork.EmailService.Models;
using SocialNetwork.Entities.User;
using SocialNetwork.Repository;
using SocialNetwork.Settings.Interfaces;

namespace SocialNetwork.AccountServices.Implementations;

public class ProfileService : IProfileService
{
    private readonly IRepository<AppUser> _userRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IMapper _mapper;
    private readonly IAppSettings _apiSettings;
    private readonly IEmailService _emailService;

    public ProfileService(IRepository<AppUser> userRepository, UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager, IMapper mapper, IAppSettings apiSettings,
        IEmailService emailService)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _apiSettings = apiSettings;
        _emailService = emailService;
    }

    public async Task<AppAccountModel> RegisterUserAsync(AppAccountModelRequest requestModel)
    {
        var user = await _userManager.FindByEmailAsync(requestModel.Email);
        ProcessException.ThrowIf(() => user is not null, ErrorMessages.UserWithThisEmailExistsError);

        user = _mapper.Map<AppUser>(requestModel);

        user.PhoneNumberConfirmed = true;
        user.EmailConfirmed = false;

        var result = await _userManager.CreateAsync(user, requestModel.Password);
        ProcessException.ThrowIf(() => !result.Succeeded, result.ToString());

        await SendConfirmRegistrationMail(user);
        await _userManager.AddToRoleAsync(user, Permissions.User.GetName());
        return _mapper.Map<AppAccountModel>(user);
    }

    /// <summary>
    /// Выдача токена
    /// </summary>
    public async Task<TokenResponse> LoginUserAsync(LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        ProcessException.ThrowIf(() => user is null, ErrorMessages.NotFoundError);

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        ProcessException.ThrowIf(() => !result.Succeeded, ErrorMessages.IncorrectEmailOrPasswordError);
        ProcessException.ThrowIf(() => user.IsBanned,
            ErrorMessages.YouBannedError); // Не даем зайти в аккаунт если забанен


        return await GetTokenResponseAsync(model, user.UserName);
    }

    /// <summary>
    /// Метод для подтверждения Email.
    /// Чтобы подтвердить Email необходимо перейти по ссылке в сообщении, которая формируется в методе SendConfirmRegistrationMail
    /// </summary>
    /// <param name="userId"> Id юзера для которого подтверждаем</param>
    /// <param name="code"> Код для подтверждения, сравниваем с тем, что сформировали и выслали в сообщении</param>
    public async Task<bool> ConfirmEmailAsync(Guid userId, string code)
    {
        code = code.Replace(' ', '+'); // почему то браузер не воспринимает символ + и вставляет пробел

        var user = await _userRepository.GetAsync(userId);

        var result = await _userManager.ConfirmEmailAsync(user, code);
        if (result.Succeeded)
        {
            user.EmailConfirmed = true;
            await _userRepository.UpdateAsync(user);
        }

        return result.Succeeded;
    }

    /// <summary>
    /// Смена пароля
    /// </summary>
    public async Task ChangePasswordAsync(Guid userId, string oldPassword, string newPassword)
    {
        var user = await _userRepository.GetAsync(userId);

        var isCorrectPassword = await _userManager.CheckPasswordAsync(user, oldPassword);
        if (isCorrectPassword)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await ResetPassword(user, newPassword, token);
        }
        else
        {
            throw new ProcessException(ErrorMessages.IncorrectPassword);
        }
    }

    /// <summary>
    /// Установка нового пароля
    /// </summary>
    private async Task ResetPassword(AppUser user, string newPassword, string resetToken)
    {
        var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
        ProcessException.ThrowIf(() => !result.Succeeded, ErrorMessages.ErrorWhileResetPassword);
    }

    /// <summary>
    /// Отправка сообщения для подтверждения почты
    /// Для подверждения генерируется ссылка на метод, в которой содержится ID пользователя и сгенерированный код.
    /// </summary>
    private async Task SendConfirmRegistrationMail(AppUser user)
    {
        user.EmailConfirmationKey = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        await _emailService.SendEmailAsync(new EmailModel
        {
            Email = user.Email,
            Message = MessageConstants.ConfirmRegistration + _apiSettings.Email.ConfirmAddress +
                      "confirm_email?userId=" + user.Id +
                      "&key=" + user.EmailConfirmationKey,
            Subject = MessageConstants.ConfirmRegistrationSubject
        });
    }

    private async Task<TokenResponse> GetTokenResponseAsync(LoginModel model, string userName)
    {
        var client = new HttpClient();
        var disco = await client.GetDiscoveryDocumentAsync(_apiSettings.Identity.Url);
        ProcessException.ThrowIf(() => disco.IsError, disco.Error);
        var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
        {
            Address = disco.TokenEndpoint,
            ClientId = model.ClientId,
            ClientSecret = model.ClientSecret,
            Password = model.Password,
            UserName = userName,
            Scope = "offline_access " + AppScopes.NetworkRead + " " + AppScopes.NetworkRead
        });

        ProcessException.ThrowIf(() => tokenResponse.IsError, tokenResponse.Error);
        return tokenResponse;
    }
}


/* Восстановление пароля
 public async Task RestorePassword(Guid userId, string newPassword, string token)
{
    var user = await _userRepository.GetAsync(userId);
    await ResetPassword(user, newPassword, token);
}

public async Task SendPasswordResetMail(string email)
{
    //A4F1304A-9DA9-4729-AEC2-D96297D85593/reset_password?token=asdasd'
    var user = (await _userRepository.GetAllAsync(x => x.Email == email)).First();
    ProcessException.ThrowIf(() => user is null, ErrorMessages.NotFoundError);
    
    string token = await _userManager.GeneratePasswordResetTokenAsync(user);
    await _emailService.SendEmailAsync(new EmailModel()
    {
        Email = user.Email,
        Message = MessageConstants.PasswordReset + _apiSettings.Email.ConfirmAddress  +
                  user.Id + "/reset_password?token="+ token,
        Subject = MessageConstants.PasswordReset
    });
}*/