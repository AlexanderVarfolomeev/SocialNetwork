using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.AccountServices;
using SocialNetwork.AccountServices.Models;
using SocialNetwork.WebAPI.Controllers.Users.Models;

namespace SocialNetwork.WebAPI.Controllers.Users;

/// <summary>
/// Контроллер для работы с учетной записью пользователя
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;
    private readonly IMapper _mapper;

    public ProfileController(IProfileService profileService, IMapper mapper)
    {
        _profileService = profileService;
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<AppAccountResponse> Register([FromBody] RegisterAccountRequest registerAccountRequest)
    {
        var modelRequest = _mapper.Map<AppAccountModelRequest>(registerAccountRequest);
        return _mapper.Map<AppAccountResponse>(await _profileService.RegisterUserAsync(modelRequest));
    }

    [HttpPost("login")]
    public async Task<string> Login([FromBody] LoginModel loginModel)
    {
        return (await _profileService.LoginUserAsync(loginModel)).AccessToken;
    }

    [HttpGet("confirm_email")]
    public async Task<bool> ConfirmEmail([FromQuery] Guid userId, [FromQuery] string key)
    {
        return await _profileService.ConfirmEmailAsync(userId, key);
    }

    [HttpPut("{id}/password")]
    public async Task<IActionResult> ChangePassword([FromRoute] Guid id, [FromBody] ChangePasswordModel model)
    {
        await _profileService.ChangePasswordAsync(id, model.OldPassword, model.NewPassword);
        return Ok();
    }
    
    //TODO закончил тут, восстановление пароля
    [HttpGet("{id}/password")]
    public async Task<IActionResult> ResetPassword([FromRoute] Guid id, [FromRoute] string key, [FromBody] string pass)
    {
        return Ok();
    }
}