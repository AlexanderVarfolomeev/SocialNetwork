using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.AccountServices.Interfaces;
using SocialNetwork.Constants.Security;

namespace SocialNetwork.WebAPI.Controllers.Users;

/// <summary>
/// Контроллер для работы с ролями
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly IAdminService _adminService;

    public RolesController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    /// <summary>
    /// Выдать роль администратора
    /// </summary>
    [Authorize(AppScopes.NetworkWrite)]
    [HttpPost("{accountId}")]
    public async Task<IActionResult> GiveAdminRole([FromRoute] Guid accountId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _adminService.GiveAdminRoleAsync(userId, accountId);
        return Ok();
    }

    /// <summary>
    /// Отозвать роль администратора
    /// </summary>
    [Authorize(AppScopes.NetworkWrite)]
    [HttpDelete("{accountId}")]
    public async Task<IActionResult> RevokeAdminRole([FromRoute] Guid accountId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _adminService.RevokeAdminRoleAsync(userId, accountId);
        return Ok();
    }
}