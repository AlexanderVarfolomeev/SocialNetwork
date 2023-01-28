using AutoMapper;
using IdentityModel.Client;
using Microsoft.AspNetCore.Identity;
using SocialNetwork.AccountServices.Models;
using SocialNetwork.Common.Enum;
using SocialNetwork.Common.Exceptions;
using SocialNetwork.Common.Security;
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

    public ProfileService(IRepository<AppUser> userRepository, UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager, IMapper mapper, IAppSettings apiSettings,
        IRepository<AppUserRole> userRolesRepository, IRepository<AppRole> roleRepository)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _apiSettings = apiSettings;
        _userRolesRepository = userRolesRepository;
        _roleRepository = roleRepository;
    }

    // TODO рассылка почты
    // TODO подтверждение почты
    // TODO подтверждение телефона
    public async Task<AppAccountModel> RegisterUserAsync(AppAccountModelRequest requestModel)
    {
        var user = await _userManager.FindByEmailAsync(requestModel.Email);
        ProcessException.ThrowIf(() => user is not null, ErrorMessages.UserWithThisEmailExistsError);

        user = _mapper.Map<AppUser>(requestModel);

        user.PhoneNumberConfirmed = false;
        user.EmailConfirmed = false;

        user.CreationDateTime = DateTimeOffset.Now;
        user.ModificationDateTime = user.CreationDateTime;
        user.Id = Guid.NewGuid();

        var result = await _userManager.CreateAsync(user, requestModel.Password);
        ProcessException.ThrowIf(() => !result.Succeeded, result.ToString());

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