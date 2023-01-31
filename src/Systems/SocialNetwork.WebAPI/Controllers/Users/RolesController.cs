using Microsoft.AspNetCore.Mvc;
using SocialNetwork.AccountServices.Interfaces;

namespace SocialNetwork.WebAPI.Controllers.Users;

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

    [HttpPost("{userId}")]
    public async Task<IActionResult> GiveAdminRole([FromRoute] Guid userId)
    {
        await _adminService.GiveAdminRoleAsync(userId);
        return Ok();
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> RevokeAdminRole([FromRoute] Guid userId)
    {
        await _adminService.RevokeAdminRoleAsync(userId);
        return Ok();
    }

}