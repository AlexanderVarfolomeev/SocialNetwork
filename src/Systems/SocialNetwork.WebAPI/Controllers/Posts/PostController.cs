using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Constants.Security;
using SocialNetwork.PostServices;
using SocialNetwork.PostServices.Models;
using SocialNetwork.WebAPI.Controllers.Posts.Models;

namespace SocialNetwork.WebAPI.Controllers.Posts;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly IMapper _mapper;

    public PostController(IPostService postService, IMapper mapper)
    {
        _postService = postService;
        _mapper = mapper;
    }

    [HttpGet("accounts/{userId}/posts")]
    public async Task<IEnumerable<PostResponse>> GetUsersPosts([FromRoute] Guid userId, [FromQuery] int offset = 0,
        [FromQuery] int limit = 10)
    {
        return _mapper.Map<IEnumerable<PostResponse>>(await _postService.GetUsersPosts(userId, offset, limit));
    }

    [HttpGet("groups/{groupId}/posts")]
    public async Task<IEnumerable<PostResponse>> GetGroupPosts([FromRoute] Guid groupId, [FromQuery] int offset = 0,
        [FromQuery] int limit = 10)
    {
        return _mapper.Map<IEnumerable<PostResponse>>(await _postService.GetGroupPosts(groupId, offset, limit));
    }

    [Authorize(AppScopes.NetworkWrite)]
    [HttpPost("posts")]
    public async Task<PostResponse> AddUsersPost([FromBody] PostRequest post)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var postModelAdd = new PostModelAdd()
        {
            CreatorId = userId,
            GroupId = null,
            IsInGroup = false,
            Text = post.Text
        };

        return _mapper.Map<PostResponse>(await _postService.AddUserPost(userId, postModelAdd));
    }

    [Authorize(AppScopes.NetworkWrite)]
    [HttpPost("{groupId}/posts")]
    public async Task<PostResponse> AddGroupPost([FromRoute] Guid groupId, [FromBody] PostRequest post)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var postModelAdd = new PostModelAdd()
        {
            CreatorId = userId,
            GroupId = groupId,
            IsInGroup = true,
            Text = post.Text
        };

        return _mapper.Map<PostResponse>(await _postService.AddGroupPost(userId, postModelAdd));
    }

    [Authorize(AppScopes.NetworkWrite)]
    [HttpPatch("posts/{postId}")]
    public async Task<PostResponse> UpdatePost([FromRoute] Guid postId, PostRequest postRequest)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        return _mapper.Map<PostResponse>(await _postService.UpdatePost(userId, postId,
            _mapper.Map<PostModelUpdate>(postRequest)));
    }

    [Authorize(AppScopes.NetworkWrite)]
    [HttpDelete("posts/{postId}")]
    public async Task<IActionResult> DeletePost([FromRoute] Guid postId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _postService.DeletePost(userId, postId);
        return Ok();
    }
}