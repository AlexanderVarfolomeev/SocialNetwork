using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.AttachmentServices;
using SocialNetwork.AttachmentServices.Models;
using SocialNetwork.Common.Enum;

namespace SocialNetwork.WebAPI.Controllers.Attachments;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/")]
[ApiController]
public class AttachmentController : ControllerBase
{
    private readonly IAttachmentService _attachmentService;

    public AttachmentController(IAttachmentService attachmentService)
    {
        _attachmentService = attachmentService;
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
    public async Task UploadAvatar (IFormFile avatar)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        await _attachmentService.UploadFiles(userId, new AttachmentModelRequest()
        {
            UserId = userId,
            FileType = FileType.Avatar,
            Files = new []{avatar}
        });
    }

    [HttpGet("accounts/{userId}/avatars")]
    public async Task<IEnumerable<string>> GetAvatars([FromRoute] Guid userId)
    {
        return await _attachmentService.GetAvatars(userId);
    }
    
    [HttpGet("accounts/{userId}/avatars/current")]
    public async Task<string> GetCurrentAvatar([FromRoute] Guid userId)
    {
        return await _attachmentService.GetCurrentAvatar(userId);
    }
}