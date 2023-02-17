using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.AttachmentServices;
using SocialNetwork.AttachmentServices.Models;
using SocialNetwork.Common.Enum;
using SocialNetwork.WebAPI.Controllers.Attachments.Models;

namespace SocialNetwork.WebAPI.Controllers.Attachments;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/")]
[ApiController]
public class AttachmentController : ControllerBase
{
    private readonly IAttachmentService _attachmentService;
    private readonly IMapper _mapper;

    public AttachmentController(IAttachmentService attachmentService, IMapper mapper)
    {
        _attachmentService = attachmentService;
        _mapper = mapper;
    }

    [HttpPost("posts/{postId}/attachments")]
    public async Task UploadFilesToPost([FromRoute] Guid postId, IEnumerable<IFormFile> attachments)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _attachmentService.UploadFiles(userId, new AttachmentModelRequest()
        {
            PostId = postId,
            FileType = FileType.Post,
            Files = attachments
        });
    }

    [HttpPost("comments/{commentId}/attachments")]
    public async Task UploadFilesToComment([FromRoute] Guid commentId, IEnumerable<IFormFile> attachments)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _attachmentService.UploadFiles(userId, new AttachmentModelRequest()
        {
            CommentId = commentId,
            FileType = FileType.Comment,
            Files = attachments
        });
    }

    [HttpPost("messages/{messageId}/attachments")]
    public async Task UploadFilesToMessage([FromRoute] Guid messageId, IEnumerable<IFormFile> attachments)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _attachmentService.UploadFiles(userId, new AttachmentModelRequest()
        {
            MessageId = messageId,
            FileType = FileType.Message,
            Files = attachments
        });
    }

    [HttpPost("profile/avatars")]
    public async Task UploadAvatar(IFormFile avatar)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        await _attachmentService.UploadFiles(userId, new AttachmentModelRequest()
        {
            UserId = userId,
            FileType = FileType.Avatar,
            Files = new[] { avatar }
        });
    }

    [HttpGet("accounts/{userId}/avatars")]
    public async Task<IEnumerable<AttachmentViewResponse>> GetAvatars([FromRoute] Guid userId)
    {
        return _mapper.Map<IEnumerable<AttachmentViewResponse>>(
            await _attachmentService.GetAttachments(FileType.Avatar, userId));
    }

    [HttpGet("posts/{postId}/attachments")]
    public async Task<IEnumerable<AttachmentViewResponse>> GetPostAttachments([FromRoute] Guid postId)
    {
        return _mapper.Map<IEnumerable<AttachmentViewResponse>>(
            await _attachmentService.GetAttachments(FileType.Post, postId));
    }

    [HttpGet("comments/{commentId}/attachments")]
    public async Task<IEnumerable<AttachmentViewResponse>> GetCommentAttachments([FromRoute] Guid commentId)
    {
        return _mapper.Map<IEnumerable<AttachmentViewResponse>>(
            await _attachmentService.GetAttachments(FileType.Comment, commentId));
    }

    [HttpGet("messages/{messageId}/attachments")]
    public async Task<IEnumerable<AttachmentViewResponse>> GetMessageAttachments([FromRoute] Guid messageId)
    {
        return _mapper.Map<IEnumerable<AttachmentViewResponse>>(
            await _attachmentService.GetAttachments(FileType.Message, messageId));
    }

    [HttpGet("accounts/{userId}/avatars/current")]
    public async Task<AttachmentViewResponse> GetCurrentAvatar([FromRoute] Guid userId)
    {
        return _mapper.Map<AttachmentViewResponse>(await _attachmentService.GetCurrentAvatar(userId));
    }

    [HttpDelete("profile/avatars/{avatarId}")]
    public async Task<IActionResult> DeleteAvatar([FromRoute] Guid avatarId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _attachmentService.DeleteAttachment(userId, FileType.Avatar, avatarId);
        return Ok();
    }

    [HttpDelete("messages/{messageId}")]
    public async Task<IActionResult> DeleteMessageAttachment([FromRoute] Guid messageId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _attachmentService.DeleteAttachment(userId, FileType.Message, messageId);
        return Ok();
    }

    [HttpDelete("posts/{postId}")]
    public async Task<IActionResult> DeletePostAttachment([FromRoute] Guid postId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _attachmentService.DeleteAttachment(userId, FileType.Post, postId);
        return Ok();
    }

    [HttpDelete("comments/{commentId}")]
    public async Task<IActionResult> DeleteCommentAttachment([FromRoute] Guid commentId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _attachmentService.DeleteAttachment(userId, FileType.Comment, commentId);
        return Ok();
    }
}