using AutoMapper;
using SocialNetwork.AccountServices.Interfaces;
using SocialNetwork.AccountServices.Models;
using SocialNetwork.Common.Exceptions;
using SocialNetwork.Constants.Errors;
using SocialNetwork.Entities.User;
using SocialNetwork.Repository;

namespace SocialNetwork.AccountServices.Implementations;

class AccountService : IAccountService
{
    private readonly IRepository<AppUser> _userRepository;
    private readonly IMapper _mapper;
    private readonly IAdminService _adminService;

    public AccountService(IRepository<AppUser> userRepository, IMapper mapper, IAdminService adminService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _adminService = adminService;
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

    public async Task<AppAccountModel> UpdateAccountAsync(Guid userId, Guid accountId, AppAccountUpdateModel model)
    {
        var userModel = _mapper.Map(model, await CheckAdminOrAccountOwner(userId, accountId));
        return _mapper.Map<AppAccountModel>(await _userRepository.UpdateAsync(userModel));
    }

    public async Task DeleteAccountAsync(Guid userId, Guid accountId)
    {
        await _userRepository.DeleteAsync(await CheckAdminOrAccountOwner(userId, accountId));
    }

    /// <summary>
    /// Проверить что действие совершает владелец аккаунта или админ
    /// </summary>
    /// <param name="userId"> Пользователь который совершает действие</param>
    /// <param name="accountId"> Id аккаунта</param>
    private async Task<AppUser> CheckAdminOrAccountOwner(Guid userId, Guid accountId)
    {
        if (userId != accountId && !await _adminService.IsAdminAsync(userId))
        {
            throw new ProcessException(ErrorMessages.OnlyAdminOrAccountOwnerCanDoIdError);
        }

        var user = await _userRepository.GetAsync(accountId);
        if (user.IsBanned)
        {
            throw new ProcessException(ErrorMessages.YouBannedError);
        }

        return user;
    }
}