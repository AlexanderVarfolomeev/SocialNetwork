using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.AccountServices.Interfaces;
using SocialNetwork.AccountServices.Models;
using SocialNetwork.AttachmentServices;
using SocialNetwork.AttachmentServices.Models;
using SocialNetwork.Common.Enum;
using SocialNetwork.Constants.Security;
using SocialNetwork.WebAPI.Controllers.CommonModels;
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
    private readonly IAttachmentService _attachmentService;

    public ProfileController(IProfileService profileService, IMapper mapper, IAttachmentService attachmentService)
    {
        _profileService = profileService;
        _mapper = mapper;
        _attachmentService = attachmentService;
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

    [HttpGet("confirm-email")]
    public async Task<bool> ConfirmEmail([FromQuery] Guid userId, [FromQuery] string key)
    {
        return await _profileService.ConfirmEmailAsync(userId, key);
    }

    [Authorize(AppScopes.NetworkWrite)]
    [HttpPut("password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _profileService.ChangePasswordAsync(userId, model.OldPassword, model.NewPassword);
        return Ok();
    }

    [Authorize(AppScopes.NetworkWrite)]
    [HttpPost("avatars")]
    public async Task<AttachmentResponse> AddAvatar(IFormFile avatar)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var createdAvatar = await _attachmentService.UploadFiles(userId, new AttachmentModelRequest()
        {
            UserId = userId,
            FileType = FileType.Avatar,
            Files = new[] { avatar }
        });

        return _mapper.Map<AttachmentResponse>(createdAvatar.First());
    }
}

/*
   [HttpPost("{id}/reset-password")]
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