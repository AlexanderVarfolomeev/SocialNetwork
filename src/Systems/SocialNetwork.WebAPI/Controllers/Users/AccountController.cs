using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.AccountServices.Interfaces;
using SocialNetwork.AccountServices.Models;
using SocialNetwork.AttachmentServices;
using SocialNetwork.Constants.Security;
using SocialNetwork.WebAPI.Controllers.Users.Models;

namespace SocialNetwork.WebAPI.Controllers.Users;

/// <summary>
/// CRUD контроллер для работы с аккаунтами пользователей
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IAccountService _accountService;
    private readonly IAdminService _adminService;
    private readonly IAttachmentService _attachmentService;

    public AccountsController(
        IMapper mapper,
        IAccountService accountService,
        IAdminService adminService,
        IAttachmentService attachmentService
    )
    {
        _mapper = mapper;
        _accountService = accountService;
        _adminService = adminService;
        _attachmentService = attachmentService;
    }

    [HttpGet("{id:Guid}")]
    public async Task<AppAccountResponse> GetAccountById([FromRoute] Guid id)
    {
        return _mapper.Map<AppAccountResponse>(await _accountService.GetAccountAsync(id));
    }

    [HttpGet("{username}")]
    public async Task<AppAccountResponse> GetAccountByUsername([FromRoute] String username)
    {
        return _mapper.Map<AppAccountResponse>(await _accountService.GetAccountAsync(username));
    }

    [HttpGet]
    public async Task<IEnumerable<AppAccountResponse>> GetAccounts([FromQuery] int offset = 0,
        [FromQuery] int limit = 10)
    {
        return _mapper.Map<IEnumerable<AppAccountResponse>>(await _accountService.GetAccountsAsync(offset, limit));
    }

    [Authorize(AppScopes.NetworkWrite)]
    [HttpPut("{accountId:Guid}")]
    public async Task<AppAccountResponse> UpdateAccount([FromRoute] Guid accountId,
        [FromBody] AppAccountUpdateRequest model)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        return _mapper.Map<AppAccountResponse>(
            await _accountService.UpdateAccountAsync(userId, accountId, _mapper.Map<AppAccountUpdateModel>(model)));
    }

    [Authorize(AppScopes.NetworkWrite)]
    [HttpDelete("{accountId}")]
    public async Task<IActionResult> DeleteAccount([FromRoute] Guid accountId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _accountService.DeleteAccountAsync(userId, accountId);
        return Ok();
    }

    [Authorize(AppScopes.NetworkWrite)]
    [HttpPut("{accountId}/ban")]
    public async Task<IActionResult> BanAccount([FromRoute] Guid accountId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _adminService.BanAccountAsync(userId, accountId);
        return Ok();
    }

    [Authorize(AppScopes.NetworkWrite)]
    [HttpPut("{accountId}/unban")]
    public async Task<IActionResult> UnbanAccount([FromRoute] Guid accountId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _adminService.UnbanAccountAsync(userId, accountId);
        return Ok();
    }

    [HttpGet("{accountId}/avatars")]
    public async Task<IEnumerable<AvatarResponse>> GetAvatars([FromRoute] Guid accountId)
    {
        var avatars = await _attachmentService.GetAvatars(accountId);
        return _mapper.Map<IEnumerable<AvatarResponse>>(avatars);
    }
}