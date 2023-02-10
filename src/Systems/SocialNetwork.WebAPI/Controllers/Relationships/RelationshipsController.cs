using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.RelationshipServices;
using SocialNetwork.RelationshipServices.Models;
using SocialNetwork.WebAPI.Controllers.Users.Models;

namespace SocialNetwork.WebAPI.Controllers.Relationships;

/// <summary>
/// Контроллер для работы с друзьями пользователя.
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class RelationshipsController : ControllerBase
{
    private readonly IRelationshipService _relationshipService;
    private readonly IMapper _mapper;

    public RelationshipsController(IRelationshipService relationshipService, IMapper mapper)
    {
        _relationshipService = relationshipService;
        _mapper = mapper;
    }

    /// <summary>
    /// Послать запрос на дружбу.
    /// </summary>
    [HttpPost("")]
    public async Task<IActionResult> SendFriendshipRequest([FromQuery] Guid recipientId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _relationshipService.SendFriendRequest(userId, recipientId);
        return Ok();
    }

    /// <summary>
    /// Принять запрос на дружбы.
    /// </summary>
    [HttpPut("accept-friendship/{requestId}")]
    public async Task<IActionResult> AcceptFriendshipRequest([FromRoute] Guid requestId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _relationshipService.AcceptFriendRequest(userId, requestId);
        return Ok();
    }

    /// <summary>
    /// Отклонить запрос на дружбу.
    /// </summary>
    [HttpPut("reject-friendship/{requestId}")]
    public async Task<IActionResult> RejectFriendshipRequest([FromRoute] Guid requestId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _relationshipService.RejectFriendRequest(userId, requestId);
        return Ok();
    }

    /// <summary>
    /// Получить список заявок в друзья.
    /// </summary>
    [HttpGet("")]
    public async Task<List<FriendshipRequest>> GetFriendshipRequests([FromQuery] int offset = 0,
        [FromQuery] int limit = 10)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        return await _relationshipService.GetFriendshipRequests(userId, offset, limit);
    }

    /// <summary>
    /// Получить список друзей.
    /// </summary>
    [HttpGet("friends")]
    public async Task<List<AppAccountResponse>> GetFriends([FromQuery] int offset = 0,
        [FromQuery] int limit = 10)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        return _mapper.Map<List<AppAccountResponse>>(await _relationshipService.GetFriendList(userId, offset, limit));
    }

    /// <summary>
    /// Удалить человека из списка друзей.
    /// </summary>
    [HttpDelete("friends/{friendId}")]
    public async Task<IActionResult> DeleteFriend([FromRoute] Guid friendId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _relationshipService.DeleteFromFriendList(userId, friendId);
        return Ok();
    }
}