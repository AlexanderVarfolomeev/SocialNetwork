using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.AccountServices.Interfaces;
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
}

/*
    * TODO закончить в клиенте
   [HttpPost("{id}/reset_password")]
   public async Task<IActionResult> ResetPassword([FromRoute] Guid id, [FromQuery] string token, [FromBody] string password)
   {
       await _profileService.RestorePassword(id, password, token);
       return Ok();
   }

   [HttpPost("forgot_password")]
   public async Task<IActionResult> ForgotPassword([FromBody] string email)
   {
       await _profileService.SendPasswordResetMail(email);
       return Ok();
   }*/