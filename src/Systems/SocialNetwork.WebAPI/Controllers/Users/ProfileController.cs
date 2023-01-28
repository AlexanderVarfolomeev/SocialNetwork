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
}