using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.AttachmentServices;
using SocialNetwork.AttachmentServices.Models;
using SocialNetwork.CommentService;
using SocialNetwork.CommentService.Models;
using SocialNetwork.Common.Enum;
using SocialNetwork.Constants.Security;
using SocialNetwork.WebAPI.Controllers.Comments.Models;
using SocialNetwork.WebAPI.Controllers.CommonModels;

namespace SocialNetwork.WebAPI.Controllers.Comments;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/")]
[ApiController]
public class CommentsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICommentService _commentService;
    private readonly IAttachmentService _attachmentService;

    public CommentsController(IMapper mapper, ICommentService commentService, IAttachmentService attachmentService)
    {
        _mapper = mapper;
        _commentService = commentService;
        _attachmentService = attachmentService;
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

    [Authorize(AppScopes.NetworkWrite)]
    [HttpDelete("comments/{commentId}")]
    public async Task<IActionResult> DeleteComment([FromRoute] Guid commentId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _commentService.DeleteComment(userId, commentId);
        return Ok();
    }

    [Authorize(AppScopes.NetworkWrite)]
    [HttpPatch("comments/{commentId}")]
    public async Task<CommentResponse> UpdateComment([FromRoute] Guid commentId, [FromBody] CommentRequest comment)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        return _mapper.Map<CommentResponse>(
            await _commentService.UpdateComment(userId, commentId, _mapper.Map<CommentModelRequest>(comment)));
    }

    /// <summary>
    /// Прикрепить изображение к комментарию
    /// </summary>
    [Authorize(AppScopes.NetworkWrite)]
    [HttpPost("comments/{commentId}/attachments")]
    public async Task<IEnumerable<AttachmentResponse>> AddAttachments([FromRoute] Guid commentId,
        IEnumerable<IFormFile> attachments)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var createdFiles = await _attachmentService.UploadFiles(userId, new AttachmentModelRequest()
        {
            CommentId = commentId,
            FileType = FileType.Comment,
            Files = attachments
        });

        return _mapper.Map<IEnumerable<AttachmentResponse>>(createdFiles);
    }

    /// <summary>
    /// Получить прикрепленные к комментарию изображения
    /// </summary>
    [HttpGet("comments/{commentId}/attachments")]
    public async Task<IEnumerable<AttachmentViewResponse>> GetAttachments([FromRoute] Guid commentId)
    {
        return _mapper.Map<IEnumerable<AttachmentViewResponse>>(
            await _attachmentService.GetAttachments(FileType.Comment, commentId));
    }
    
    /// <summary>
    /// Удалить прикрепленное изображение
    /// </summary>
    [Authorize(AppScopes.NetworkWrite)]
    [HttpDelete("comments/{commentId}/attachments/{attachmentId}")]
    public async Task<IActionResult> DeleteAttachment([FromRoute] Guid commentId, [FromRoute] Guid attachmentId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _attachmentService.DeleteCommentAttachment(userId, commentId, attachmentId);
        return Ok();
    }
}