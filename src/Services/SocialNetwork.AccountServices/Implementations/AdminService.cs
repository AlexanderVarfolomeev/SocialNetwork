using SocialNetwork.AccountServices.Interfaces;
using SocialNetwork.Common.Enum;
using SocialNetwork.Common.Exceptions;
using SocialNetwork.Constants.Errors;
using SocialNetwork.Entities.User;
using SocialNetwork.Repository;

namespace SocialNetwork.AccountServices.Implementations;

public class AdminService : IAdminService
{
    private readonly IRepository<AppUser> _userRepository;
    private readonly IRepository<AppUserRole> _userRolesRepository;
    private readonly IRepository<AppRole> _roleRepository;

    public AdminService(IRepository<AppUser> userRepository,
        IRepository<AppUserRole> userRolesRepository,
        IRepository<AppRole> roleRepository
    )
    {
        _userRepository = userRepository;
        _userRolesRepository = userRolesRepository;
        _roleRepository = roleRepository;
    }


    public async Task<bool> IsAdminAsync(Guid userId)
    {
        var userRoles = await _userRolesRepository.GetAllAsync(x => x.UserId == userId);
        return userRoles.Any(x => x.Role.Permissions == Permissions.Admin || x.Role.Permissions == Permissions.GodAdmin);
    }

    public async Task GiveAdminRoleAsync(Guid userId, Guid accountId)
    {
        if (!await IsAdminAsync(userId))
            throw new ProcessException(ErrorMessages.OnlyAdminCanDoItError);

        var user = await _userRepository.GetAsync(accountId);
        ProcessException.ThrowIf(() => user.IsBanned, ErrorMessages.UserIsBannedError);

        if (await IsAdminAsync(userId))
            throw new ProcessException(ErrorMessages.UserIsAdminError);

        var adminRole = await _roleRepository.GetAsync(x => x.Permissions == Permissions.Admin);
        var newUserRole = new AppUserRole { RoleId = adminRole.Id, UserId = userId };

        await _userRolesRepository.AddAsync(newUserRole);
    }

    public async Task RevokeAdminRoleAsync(Guid userId, Guid accountId)
    {
        if (!await IsAdminAsync(userId))
            throw new ProcessException(ErrorMessages.OnlyAdminCanDoItError);

        var isUserExists = await _userRepository.Any(accountId);
        ProcessException.ThrowIf(() => !isUserExists, ErrorMessages.NotFoundError);

        if (!await IsAdminAsync(userId))
            throw new ProcessException(ErrorMessages.UserIsNotAdminError);

        var userAdminRole = await _userRolesRepository.GetAsync(x =>
            x.UserId == userId && x.Role.Permissions == Permissions.Admin);

        await _userRolesRepository.DeleteAsync(userAdminRole);
    }

    public async Task BanAccountAsync(Guid userId, Guid accountId)
    {
        if (!await IsAdminAsync(userId))
            throw new ProcessException(ErrorMessages.OnlyAdminCanDoItError);

        var user = await _userRepository.GetAsync(accountId);
        ProcessException.ThrowIf(() => user.IsBanned, ErrorMessages.UserIsBannedError);

        if (await IsAdminAsync(userId))
            throw new ProcessException(ErrorMessages.UserIsAdminError);

        user.IsBanned = true;
        await _userRepository.UpdateAsync(user);
    }

    public async Task UnbanAccountAsync(Guid userId, Guid accountId)
    {
        if (!await IsAdminAsync(userId))
            throw new ProcessException(ErrorMessages.OnlyAdminCanDoItError);

        var user = await _userRepository.GetAsync(accountId);

        ProcessException.ThrowIf(() => !user.IsBanned, ErrorMessages.UserIsUnbannedError);

        user.IsBanned = false;
        await _userRepository.UpdateAsync(user);
    }
}