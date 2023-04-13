using Microsoft.AspNetCore.Identity;
using SocialNetwork.AccountServices.Interfaces;
using SocialNetwork.Cache;
using SocialNetwork.Common.Enum;
using SocialNetwork.Common.Exceptions;
using SocialNetwork.Common.Extensions;
using SocialNetwork.Constants.Errors;
using SocialNetwork.Entities.User;
using SocialNetwork.Repository;

namespace SocialNetwork.AccountServices.Implementations;

public class AdminService : IAdminService
{
    private readonly string _accountsCacheKey = "accounts_cache_key";
    private readonly IRepository<AppUser> _userRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly ICacheService _cacheService;

    public AdminService(IRepository<AppUser> userRepository,
        UserManager<AppUser> userManager, ICacheService cacheService)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _cacheService = cacheService;
    }

    public async Task<bool> IsAdminAsync(Guid userId)
    {
        var user = await _userRepository.GetAsync(userId);
        return await _userManager.IsInRoleAsync(user, Permissions.Admin.GetName()) ||
               await _userManager.IsInRoleAsync(user, Permissions.GodAdmin.GetName());
    }

    public async Task GiveAdminRoleAsync(Guid userId, Guid accountId)
    {
        if (!await IsAdminAsync(userId))
            throw new ProcessException(HttpErrorsCode.Forbidden, ErrorMessages.OnlyAdminCanDoItError);

        var user = await _userRepository.GetAsync(accountId);
        ProcessException.ThrowIf(() => user.IsBanned, ErrorMessages.UserIsBannedError, HttpErrorsCode.Forbidden);

        if (await IsAdminAsync(userId))
            throw new ProcessException(HttpErrorsCode.Forbidden, ErrorMessages.UserIsAdminError);

        await _userManager.AddToRoleAsync(user, Permissions.Admin.GetName());
        await _cacheService.Delete(_accountsCacheKey);
    }

    public async Task RevokeAdminRoleAsync(Guid userId, Guid accountId)
    {
        if (!await IsAdminAsync(userId))
            throw new ProcessException(HttpErrorsCode.Forbidden, ErrorMessages.OnlyAdminCanDoItError);

        var user = await _userRepository.GetAsync(accountId);

        if (!await IsAdminAsync(userId))
            throw new ProcessException(HttpErrorsCode.Forbidden, ErrorMessages.UserIsNotAdminError);

        await _userManager.RemoveFromRoleAsync(user, Permissions.Admin.GetName());
        await _cacheService.Delete(_accountsCacheKey);
    }

    public async Task BanAccountAsync(Guid userId, Guid accountId)
    {
        if (!await IsAdminAsync(userId))
            throw new ProcessException(HttpErrorsCode.Forbidden, ErrorMessages.OnlyAdminCanDoItError);

        var user = await _userRepository.GetAsync(accountId);
        ProcessException.ThrowIf(() => user.IsBanned, ErrorMessages.UserIsBannedError, HttpErrorsCode.Forbidden);

        if (await IsAdminAsync(userId))
            throw new ProcessException(HttpErrorsCode.Forbidden, ErrorMessages.CantBanAdminError);

        user.IsBanned = true;
        await _userRepository.UpdateAsync(user);
        await _cacheService.Delete(_accountsCacheKey);
    }

    public async Task UnbanAccountAsync(Guid userId, Guid accountId)
    {
        if (!await IsAdminAsync(userId))
            throw new ProcessException(HttpErrorsCode.Forbidden, ErrorMessages.OnlyAdminCanDoItError);

        var user = await _userRepository.GetAsync(accountId);

        ProcessException.ThrowIf(() => !user.IsBanned, ErrorMessages.UserIsUnbannedError, HttpErrorsCode.BadRequest);

        user.IsBanned = false;
        await _userRepository.UpdateAsync(user);
    }
}