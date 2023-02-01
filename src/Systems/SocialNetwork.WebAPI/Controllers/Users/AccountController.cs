using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.AccountServices.Interfaces;
using SocialNetwork.AccountServices.Models;
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

    public AccountsController(IMapper mapper,
        IAccountService accountService,
        IAdminService adminService)
    {
        _mapper = mapper;
        _accountService = accountService;
        _adminService = adminService;
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
    [HttpPut("{id:Guid}")]
    public async Task<AppAccountResponse> UpdateAccount([FromRoute] Guid id, [FromBody] AppAccountUpdateRequest model)
    {
        return _mapper.Map<AppAccountResponse>(
            await _accountService.UpdateAccountAsync(id, _mapper.Map<AppAccountUpdateModel>(model)));
    }
    
    [Authorize(AppScopes.NetworkWrite)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccount([FromRoute] Guid id)
    {
        await _accountService.DeleteAccountAsync(id);
        return Ok();
    }

    [Authorize(AppScopes.NetworkWrite)]
    [HttpPut("{id}/ban")]
    public async Task<IActionResult> BanAccount([FromRoute] Guid id)
    {
        await _adminService.BanUserAsync(id);
        return Ok();
    }
    
    [Authorize(AppScopes.NetworkWrite)]
    [HttpPut("{id}/unban")]
    public async Task<IActionResult> UnbanAccount([FromRoute] Guid id)
    {
        await _adminService.UnbanUserAsync(id);
        return Ok();
    }
}