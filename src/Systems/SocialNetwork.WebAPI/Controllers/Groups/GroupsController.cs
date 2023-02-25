using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Constants.Security;
using SocialNetwork.GroupServices;
using SocialNetwork.GroupServices.Models;
using SocialNetwork.WebAPI.Controllers.Groups.Models;

namespace SocialNetwork.WebAPI.Controllers.Groups;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/")]
[ApiController]
public class GroupsController : ControllerBase
{
    private readonly IGroupService _groupService;
    private readonly IMapper _mapper;

    public GroupsController(
        IGroupService groupService,
        IMapper mapper)
    {
        _groupService = groupService;
        _mapper = mapper;
    }

    /// <summary>
    /// Получить список групп
    /// </summary>
    [HttpGet("groups")]
    public async Task<IEnumerable<GroupResponse>> GetGroups([FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        return _mapper.Map<IEnumerable<GroupResponse>>(await _groupService.GetGroups(offset, limit));
    }

    /// <summary>
    /// Получить группу по имени
    /// </summary>
    [HttpGet("groups/{groupName}")]
    public async Task<GroupResponse> GetGroupByName([FromRoute] string groupName)
    {
        return _mapper.Map<GroupResponse>(await _groupService.GetGroupByName(groupName));
    }

    /// <summary>
    /// Создать группу
    /// </summary>
    [Authorize(AppScopes.NetworkWrite)]
    [HttpPost("groups")]
    public async Task<GroupResponse> CreateGroup([FromBody] GroupRequest group)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        return _mapper.Map<GroupResponse>(
            await _groupService.CreateGroup(userId, _mapper.Map<GroupModelRequest>(group)));
    }

    /// <summary>
    /// Получить подписчиков группы
    /// </summary>
    [HttpGet("groups/{groupId}/subscribes")]
    public async Task<IEnumerable<UserInGroupResponse>> GetSubscribers([FromRoute] Guid groupId)
    {
        return _mapper.Map<IEnumerable<UserInGroupResponse>>(await _groupService.GetSubscribers(groupId));
    }

    /// <summary>
    /// Подписаться на группу
    /// </summary>
    [Authorize(AppScopes.NetworkWrite)]
    [HttpPost("groups/{groupId}/subscribes")]
    public async Task<IActionResult> SubscribeToGroup([FromRoute] Guid groupId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _groupService.SubscribeToGroup(userId, groupId);
        return Ok();
    }

    [Authorize(AppScopes.NetworkWrite)]
    [HttpPost("groups/{groupId}/admins/{receiverId}")]
    public async Task<IActionResult> GrantAdminRole([FromRoute] Guid groupId, [FromRoute] Guid receiverId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _groupService.GrantAdminRole(userId, receiverId, groupId);
        return Ok();
    }
    
    [Authorize(AppScopes.NetworkWrite)]
    [HttpDelete("groups/{groupId}/admins/{receiverId}")]
    public async Task<IActionResult> RevokeAdminRole([FromRoute] Guid groupId, [FromRoute] Guid receiverId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _groupService.RevokeAdminRole(userId, receiverId, groupId);
        return Ok();
    }
}