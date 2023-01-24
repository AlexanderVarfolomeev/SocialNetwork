using System.Diagnostics;
using AutoMapper;
using IdentityModel.Client;
using Microsoft.AspNetCore.Identity;
using SocialNetwork.AccountServices.Models;
using SocialNetwork.Common.Enum;
using SocialNetwork.Common.Exceptions;
using SocialNetwork.Entities.User;
using SocialNetwork.Repository;

namespace SocialNetwork.AccountServices;

public class ProfileService : IProfileService
{
    private readonly IRepository<AppUser> _userRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IMapper _mapper;
    private readonly IRepository<AppUserRole> _userRolesRepository;
    private readonly IRepository<AppRole> _roleRepository;

    public ProfileService(IRepository<AppUser> userRepository, UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager, IMapper mapper,
        IRepository<AppUserRole> userRolesRepository, IRepository<AppRole> roleRepository)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _userRolesRepository = userRolesRepository;
        _roleRepository = roleRepository;
    }

    // TODO рассылка почты
    // TODO подтверждение почты
    // TODO подтверждение телефона
    public async Task<AppAccountModel> RegisterUser(AppAccountModelRequest requestModel)
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

    public Task<TokenResponse> LoginUser(LoginModel model)
    {
        throw new NotImplementedException();
    }

    private async Task GiveUserRole(AppUser user)
    {
        var userRoleId = (await _roleRepository.GetAllAsync(x => x.Permissions == Permissions.User)).First().Id;
        await _userRolesRepository.AddAsync(new AppUserRole() { RoleId = userRoleId, UserId = user.Id });
    }
}