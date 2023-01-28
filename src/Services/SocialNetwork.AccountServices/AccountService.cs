using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing.Template;
using SocialNetwork.AccountServices.Models;
using SocialNetwork.Common.Enum;
using SocialNetwork.Common.Exceptions;
using SocialNetwork.Entities.User;
using SocialNetwork.Repository;
using SocialNetwork.Settings.Interfaces;

namespace SocialNetwork.AccountServices;

//TODO действия админов
class AccountService : IAccountService
{
    private readonly IRepository<AppUser> _userRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IMapper _mapper;
    private readonly IAppSettings _apiSettings;
    private readonly IRepository<AppUserRole> _userRolesRepository;
    private readonly IRepository<AppRole> _roleRepository;


    public AccountService(IRepository<AppUser> userRepository, UserManager<AppUser> userManager,
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

    public async Task<AppAccountModel> GetAccountAsync(Guid id)
    {
        return _mapper.Map<AppAccountModel>(await GetUserAsync(id));
    }

    public async Task<AppAccountModel> GetAccountAsync(string username)
    {
        try
        {
            return _mapper.Map<AppAccountModel>(
                (await _userRepository.GetAllAsync((x) => x.UserName == username)).First());
        }
        catch (ProcessException exc)
        {
            throw new ProcessException(ErrorMessages.NotFoundError, exc);
        }
    }

    public async Task<IEnumerable<AppAccountModel>> GetAccountsAsync(int offset = 0, int limit = 10)
    {
        return _mapper.Map<IEnumerable<AppAccountModel>>(await _userRepository.GetAllAsync(offset, limit));
    }

    public async Task<AppAccountModel> UpdateAccountAsync(Guid id, AppAccountUpdateModel model)
    {
        var user = await GetUserAsync(id);
        var userModel = _mapper.Map(model, user);
        return _mapper.Map<AppAccountModel>(await _userRepository.UpdateAsync(userModel));
    }

    public async Task DeleteAccountAsync(Guid id)
    {
        var user = await GetUserAsync(id);
        await _userRepository.DeleteAsync(user);
    }

    public async Task BanUserAsync(Guid id)
    {
        var user = await GetUserAsync(id);
        user.IsBanned = true;
        await _userRepository.UpdateAsync(user);
    }

    public async Task<bool> IsUserBanned(Guid id)
    {
        return (await GetUserAsync(id)).IsBanned;
    }

    public async Task<bool> IsAdmin(Guid id)
    {
        var roles = await _userRolesRepository.GetAllAsync((x) => x.UserId == id);
        return roles.Any(x => x.Role.Permissions == Permissions.Admin || x.Role.Permissions == Permissions.GodAdmin);
    }

    private async Task<AppUser> GetUserAsync(Guid id)
    {
        try
        {
            return await _userRepository.GetAsync(id);
        }
        catch (ProcessException exc)
        {
            throw new ProcessException(ErrorMessages.NotFoundError, exc);
        }
    }
}