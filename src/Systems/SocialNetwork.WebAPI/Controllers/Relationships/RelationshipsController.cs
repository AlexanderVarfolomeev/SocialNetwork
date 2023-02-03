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
    public async Task<IActionResult> SendFreindshipRequest([FromQuery] Guid userId)
    {
        await _relationshipService.SendFriendRequest(userId);
        return Ok();
    }

    /// <summary>
    /// Принять запрос на дружбы.
    /// </summary>
    [HttpPut("accept-friendship/{id}")]
    public async Task<IActionResult> AcceptFriendshipRequest([FromRoute] Guid id)
    {
        await _relationshipService.AcceptFriendRequest(id);
        return Ok();
    }

    /// <summary>
    /// Отклонить запрос на дружбу.
    /// </summary>
    [HttpPut("reject-friendship/{id}")]
    public async Task<IActionResult> RejectFriendshipRequest([FromRoute] Guid id)
    {
        await _relationshipService.RejectFriendRequest(id);
        return Ok();
    }

    /// <summary>
    /// Получить список заявок в друзья.
    /// </summary>
    [HttpGet("")]
    public async Task<List<FriendshipRequest>> GetFriendshipRequests([FromQuery] int offset = 0,
        [FromQuery] int limit = 10)
    {
        return await _relationshipService.GetFriendshipRequests(offset, limit);
    }

    /// <summary>
    /// Получить список друзей.
    /// </summary>
    [HttpGet("friends")]
    public async Task<List<AppAccountResponse>> GetFriends([FromQuery] int offset = 0,
        [FromQuery] int limit = 10)
    {
        return _mapper.Map<List<AppAccountResponse>>(await _relationshipService.GetFriendList(offset, limit));
    }

    /// <summary>
    /// Удалить человека из списка друзей.
    /// </summary>
    [HttpDelete("friends/{id}")]
    public async Task<IActionResult> DeleteFriend([FromRoute] Guid id)
    {
        await _relationshipService.DeleteFromFriendList(id);
        return Ok();
    }
}