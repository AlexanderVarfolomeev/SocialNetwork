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

    [HttpGet("groups")]
    public async Task<IEnumerable<GroupResponse>> GetGroups([FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        return _mapper.Map<IEnumerable<GroupResponse>>(await _groupService.GetGroups(offset, limit));
    }

    [HttpGet("groups/{groupName}")]
    public async Task<GroupResponse> GetGroupByName([FromRoute] string groupName)
    {
        return _mapper.Map<GroupResponse>(await _groupService.GetGroupByName(groupName));
    }
    
    [Authorize(AppScopes.NetworkWrite)]
    [HttpPost("groups")]
    public async Task<GroupResponse> CreateGroup([FromBody] GroupRequest group)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        return _mapper.Map<GroupResponse>(
            await _groupService.CreateGroup(userId, _mapper.Map<GroupModelRequest>(group)));
    }
}