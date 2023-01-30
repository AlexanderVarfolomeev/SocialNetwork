using AutoMapper;
using IdentityModel.Client;
using Microsoft.AspNetCore.Identity;
using SocialNetwork.AccountServices.Models;
using SocialNetwork.Common.Email;
using SocialNetwork.Common.Enum;
using SocialNetwork.Common.Exceptions;
using SocialNetwork.Common.Security;
using SocialNetwork.EmailService;
using SocialNetwork.EmailService.Models;
using SocialNetwork.Entities.User;
using SocialNetwork.Repository;
using SocialNetwork.Settings.Interfaces;

namespace SocialNetwork.AccountServices;

public class ProfileService : IProfileService
{
    private readonly IRepository<AppUser> _userRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IMapper _mapper;
    private readonly IAppSettings _apiSettings;
    private readonly IRepository<AppUserRole> _userRolesRepository;
    private readonly IRepository<AppRole> _roleRepository;
    private readonly IEmailService _emailService;

    public ProfileService(IRepository<AppUser> userRepository, UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager, IMapper mapper, IAppSettings apiSettings,
        IRepository<AppUserRole> userRolesRepository, IRepository<AppRole> roleRepository,
        IEmailService emailService)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _apiSettings = apiSettings;
        _userRolesRepository = userRolesRepository;
        _roleRepository = roleRepository;
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
        await GiveUserRole(user);
        return _mapper.Map<AppAccountModel>(user);
    }

    public async Task<TokenResponse> LoginUserAsync(LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        ProcessException.ThrowIf(() => user is null, ErrorMessages.NotFoundError);

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        ProcessException.ThrowIf(() => !result.Succeeded, ErrorMessages.IncorrectEmailOrPasswordError);


        return await GetTokenResponseAsync(model, user.UserName);
    }

    public async Task<bool> ConfirmEmailAsync(Guid userId, string code)
    {
        code = code.Replace(' ', '+'); // почему то браузер не воспринимает символ + и вставляет пробел
        var user = await _userRepository.GetAsync(userId);
        ProcessException.ThrowIf(() => user is null, ErrorMessages.NotFoundError);
        var result = await _userManager.ConfirmEmailAsync(user, code);
        if (result.Succeeded)
        {
            user.EmailConfirmed = true;
            await _userRepository.UpdateAsync(user);
            return true;
        }

        return false;
    }

    public async Task ChangePasswordAsync(Guid userId, string oldPassword, string newPassword)
    {
        var user = await _userRepository.GetAsync(userId);
        ProcessException.ThrowIf(() => user is null, ErrorMessages.NotFoundError);

        var isCorrectPassword = await _userManager.CheckPasswordAsync(user, oldPassword);
        if (isCorrectPassword)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await ResetPassword(user, newPassword, token);
        }
        else
            throw new ProcessException(ErrorMessages.IncorrectPassword);
    }

    //TODO закончил тут, восстановление пароля
    public async Task SendPasswordResetMail(AppUser user)
    {
       string key = await _userManager.GeneratePasswordResetTokenAsync(user);
        await _emailService.SendEmailAsync(new EmailModel()
        {
            Email = user.Email,
            Message = MessageConstants.PasswordReset + _apiSettings.Email.ConfirmAddress + "password/userId=" + user.Id +
                      "&key=" + key,
            Subject = MessageConstants.ConfirmRegistrationSubject
        });
    }
    private async Task ResetPassword(AppUser user, string newPassword, string resetToken)
    {
        var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
        ProcessException.ThrowIf(() => !result.Succeeded, ErrorMessages.ErrorWhileResetPassword);
    }    
    /// <summary>
    /// Отправка сообщения для подтверждения почты
    /// </summary>
    /// <param name="user"></param>
    private async Task SendConfirmRegistrationMail(AppUser user)
    {
        user.EmailConfirmationKey = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        await _emailService.SendEmailAsync(new EmailModel()
        {
            Email = user.Email,
            Message = MessageConstants.ConfirmRegistration + _apiSettings.Email.ConfirmAddress + "confirm-email?userId=" + user.Id +
                      "&key=" + user.EmailConfirmationKey,
            Subject = MessageConstants.ConfirmRegistrationSubject
        });
    }

    /// <summary>
    /// Выдача прав пользователя
    /// </summary>
    /// <param name="user"></param>
    private async Task GiveUserRole(AppUser user)
    {
        var userRoleId = (await _roleRepository.GetAllAsync(x => x.Permissions == Permissions.User)).First().Id;
        await _userRolesRepository.AddAsync(new AppUserRole() { RoleId = userRoleId, UserId = user.Id });
    }

    private async Task<TokenResponse> GetTokenResponseAsync(LoginModel model, string userName)
    {
        var client = new HttpClient();
        var disco = await client.GetDiscoveryDocumentAsync(_apiSettings.Identity.Url);
        ProcessException.ThrowIf(() => disco.IsError, disco.Error);
        var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest()
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