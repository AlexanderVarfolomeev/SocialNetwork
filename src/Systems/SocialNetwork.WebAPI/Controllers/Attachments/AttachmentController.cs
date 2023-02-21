using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.AttachmentServices;
using SocialNetwork.AttachmentServices.Models;
using SocialNetwork.Common.Enum;
using SocialNetwork.Constants.Security;
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

    [Authorize(AppScopes.NetworkWrite)]
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

    [Authorize(AppScopes.NetworkWrite)]
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

    [Authorize(AppScopes.NetworkWrite)]
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

    [Authorize(AppScopes.NetworkWrite)]
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

    //TODO Что то напутал, исправить удаление, передаются неправильные id, неправильный путь, должен быть posts/postId/attachmentId
    /*[Authorize(AppScopes.NetworkWrite)]
    [HttpDelete("profile/avatars/{avatarId}")]
    public async Task<IActionResult> DeleteAvatar([FromRoute] Guid avatarId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _attachmentService.DeleteAttachment(userId, FileType.Avatar, avatarId);
        return Ok();
    }

    [Authorize(AppScopes.NetworkWrite)]
    [HttpDelete("messages/{attachmentId}")]
    public async Task<IActionResult> DeleteMessageAttachment([FromRoute] Guid attachmentId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _attachmentService.DeleteAttachment(userId, FileType.Message, attachmentId);
        return Ok();
    }

    [Authorize(AppScopes.NetworkWrite)]
    [HttpDelete("posts/{attachmentId}")]
    public async Task<IActionResult> DeletePostAttachment([FromRoute] Guid attachmentId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _attachmentService.DeleteAttachment(userId, FileType.Post, attachmentId);
        return Ok();
    }

    [Authorize(AppScopes.NetworkWrite)]
    [HttpDelete("comments/{attachmentId}")]
    public async Task<IActionResult> DeleteCommentAttachment([FromRoute] Guid attachmentId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _attachmentService.DeleteAttachment(userId, FileType.Comment, attachmentId);
        return Ok();
    }*/
}