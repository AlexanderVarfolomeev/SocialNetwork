using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SocialNetwork.AccountServices.Interfaces;
using SocialNetwork.Common.Enum;
using SocialNetwork.Common.Exceptions;
using SocialNetwork.Constants.Errors;
using SocialNetwork.Entities.User;
using SocialNetwork.Repository;
using SocialNetwork.Settings.Interfaces;

namespace SocialNetwork.AccountServices.Implementations;

public class AdminService : IAdminService
{
    #region Ctor

    private readonly IRepository<AppUser> _userRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IMapper _mapper;
    private readonly IAppSettings _apiSettings;
    private readonly IRepository<AppUserRole> _userRolesRepository;
    private readonly IRepository<AppRole> _roleRepository;

    private readonly Guid _currentUserId;

    public AdminService(IRepository<AppUser> userRepository,
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IMapper mapper,
        IAppSettings apiSettings,
        IRepository<AppUserRole> userRolesRepository,
        IRepository<AppRole> roleRepository,
        IHttpContextAccessor accessor
    )
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _apiSettings = apiSettings;
        _userRolesRepository = userRolesRepository;
        _roleRepository = roleRepository;


        var value = accessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _currentUserId = value is null ? Guid.Empty : Guid.Parse(value);
    }

    #endregion

    public async Task<bool> IsAdminAsync(Guid userId)
    {
        var roles = await _userRolesRepository.GetAllAsync((x) => x.UserId == userId);
        return roles.Any(x => x.Role.Permissions == Permissions.Admin || x.Role.Permissions == Permissions.GodAdmin);
    }

    public async Task GiveAdminRoleAsync(Guid userId)
    {
        await IsCurrentUserAdmin();

        var user = await _userRepository.GetAsync(userId);

        if (user.IsBanned)
        {
            throw new ProcessException(ErrorMessages.UserIsBannedError);
        }

        if (await IsAdminAsync(userId))
        {
            throw new ProcessException(ErrorMessages.UserIsAdminError);
        }
        
        var adminRole = await _roleRepository.GetAllAsync(x => x.Permissions == Permissions.Admin);
        AppUserRole role = new AppUserRole() { RoleId = adminRole.First().Id, UserId = userId };
        role.Init();
        await _userRolesRepository.AddAsync(role);

    }

    public async Task RevokeAdminRoleAsync(Guid userId)
    {
        await IsCurrentUserAdmin();

        var user = await _userRepository.GetAsync(userId);

        if (!await IsAdminAsync(userId))
        {
            throw new ProcessException(ErrorMessages.UserIsNotAdminError);
        }
        
        var role = await _userRolesRepository.GetAllAsync(x => x.UserId == userId && x.Role.Permissions == Permissions.Admin);
        await _userRolesRepository.DeleteAsync(role.First());
    }

    public async Task BanUserAsync(Guid userId)
    {
        await IsCurrentUserAdmin();
        
        var user = await _userRepository.GetAsync(userId);

        if (user.IsBanned)
        {
            throw new ProcessException(ErrorMessages.UserIsBannedError);
        }

        if (await IsAdminAsync(userId))
        {
            throw new ProcessException(ErrorMessages.UserIsAdminError);
        }

        user.IsBanned = true;
        await _userRepository.UpdateAsync(user);
    }

    public async Task UnbanUserAsync(Guid userId)
    {
        await IsCurrentUserAdmin();
        
        var user = await _userRepository.GetAsync(userId);

        if (!user.IsBanned)
        {
            throw new ProcessException(ErrorMessages.UserIsUnbannedError);
        }

        user.IsBanned = false;
        await _userRepository.UpdateAsync(user);
    }

    private async Task IsCurrentUserAdmin()
    {
        if (!await IsAdminAsync(_currentUserId))
        {
            throw new ProcessException(ErrorMessages.OnlyAdminCanDoItError);
        }
    }
}