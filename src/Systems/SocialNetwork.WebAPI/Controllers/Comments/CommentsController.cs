using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.CommentService;
using SocialNetwork.CommentService.Models;
using SocialNetwork.Constants.Security;
using SocialNetwork.WebAPI.Controllers.Comments.Models;

namespace SocialNetwork.WebAPI.Controllers.Comments;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/")]
[ApiController]
public class CommentsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICommentService _commentService;

    public CommentsController(IMapper mapper, ICommentService commentService)
    {
        _mapper = mapper;
        _commentService = commentService;
    }

    [HttpGet("posts/{postId}/comments")]
    public async Task<IEnumerable<CommentResponse>> GetCommentsByPostId([FromRoute] Guid postId)
    {
        var posts = await _commentService.GetCommentsByPost(postId);
        return _mapper.Map<IEnumerable<CommentResponse>>(posts);
    }

    [Authorize(AppScopes.NetworkWrite)]
    [HttpPost("posts/{postId}/comments")]
    public async Task<CommentResponse> AddComment([FromRoute] Guid postId, [FromBody] CommentRequest comment)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        return _mapper.Map<CommentResponse>(await _commentService.AddComment(userId, postId,
            _mapper.Map<CommentModelRequest>(comment)));
    }
}