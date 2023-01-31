using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SocialNetwork.AccountServices.Interfaces;
using SocialNetwork.AccountServices.Models;
using SocialNetwork.Common.Exceptions;
using SocialNetwork.Entities.User;
using SocialNetwork.Repository;
using SocialNetwork.Settings.Interfaces;

namespace SocialNetwork.AccountServices.Implementations;

class AccountService : IAccountService
{
    private readonly IRepository<AppUser> _userRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IMapper _mapper;
    private readonly IAppSettings _apiSettings;
    private readonly IRepository<AppUserRole> _userRolesRepository;
    private readonly IRepository<AppRole> _roleRepository;
    private readonly IAdminService _adminService;

    private readonly Guid _currentUserId;

    public AccountService(IRepository<AppUser> userRepository, UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager, IMapper mapper, IAppSettings apiSettings, IHttpContextAccessor accessor,
        IRepository<AppUserRole> userRolesRepository, IRepository<AppRole> roleRepository, IAdminService adminService)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _apiSettings = apiSettings;
        _userRolesRepository = userRolesRepository;
        _roleRepository = roleRepository;
        _adminService = adminService;

        var value = accessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _currentUserId = value is null ? Guid.Empty : Guid.Parse(value);
    }

    public async Task<AppAccountModel> GetAccountAsync(Guid id)
    {
        return _mapper.Map<AppAccountModel>(await _userRepository.GetAsync(id));
    }

    public async Task<AppAccountModel> GetAccountAsync(string username)
    {
        return _mapper.Map<AppAccountModel>(await _userRepository.GetAsync(x => x.UserName == username));
    }

    public async Task<IEnumerable<AppAccountModel>> GetAccountsAsync(int offset = 0, int limit = 10)
    {
        return _mapper.Map<IEnumerable<AppAccountModel>>(await _userRepository.GetAllAsync(offset, limit));
    }

    public async Task<AppAccountModel> UpdateAccountAsync(Guid id, AppAccountUpdateModel model)
    {
        var userModel = _mapper.Map(model, await CheckAdminOrAccountOwner(id));
        return _mapper.Map<AppAccountModel>(await _userRepository.UpdateAsync(userModel));
    }

    public async Task DeleteAccountAsync(Guid id)
    {
        await _userRepository.DeleteAsync(await CheckAdminOrAccountOwner(id));
    }

    private async Task<AppUser> CheckAdminOrAccountOwner(Guid id)
    {
        if (_currentUserId != id && !await _adminService.IsAdminAsync(_currentUserId))
        {
            throw new ProcessException(ErrorMessages.OnlyAdminOrAccountOwnerCanDoIdError);
        }

        var user = await _userRepository.GetAsync(id);
        if (user.IsBanned)
        {
            throw new ProcessException(ErrorMessages.YouBannedError);
        }

        return user;
    }
}