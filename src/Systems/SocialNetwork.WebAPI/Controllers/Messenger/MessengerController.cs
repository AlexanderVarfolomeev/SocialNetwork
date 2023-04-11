using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.AttachmentServices;
using SocialNetwork.AttachmentServices.Models;
using SocialNetwork.Common.Enum;
using SocialNetwork.MessengerService;
using SocialNetwork.MessengerService.Models;
using SocialNetwork.WebAPI.Controllers.CommonModels;
using SocialNetwork.WebAPI.Controllers.Messenger.Models;

namespace SocialNetwork.WebAPI.Controllers.Messenger;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/")]
[ApiController]
[Authorize]
public class MessengerController : ControllerBase
{
    private readonly IMessengerService _messengerService;
    private readonly IMapper _mapper;
    private readonly IAttachmentService _attachmentService;

    public MessengerController(IMessengerService messengerService, IMapper mapper, IAttachmentService attachmentService)
    {
        _messengerService = messengerService;
        _mapper = mapper;
        _attachmentService = attachmentService;
    }

    [HttpPost("messenger/{receiverId}")]
    public async Task<MessageResponse> SendMessage([FromRoute] Guid receiverId, [FromBody] string text)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var res = await _messengerService.SendMessageToUser(userId, receiverId,
            new MessageModelRequest { Text = text });
        return _mapper.Map<MessageResponse>(res);
    }

    [HttpGet("messenger/chats")]
    public async Task<IEnumerable<ChatResponse>> GetChats()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        return _mapper.Map<IEnumerable<ChatResponse>>(await _messengerService.GetChats(userId));
    }


    [HttpGet("messenger/chats/{chatId}")]
    public async Task<IEnumerable<MessageResponse>> GetMessages([FromRoute] Guid chatId, [FromQuery] int offset = 0,
        [FromQuery] int limit = 1000)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        return _mapper.Map<IEnumerable<MessageResponse>>(await _messengerService.GetMessages(userId, chatId, offset, limit));
    }
    
    [HttpGet("messenger/chats/{chatId}/users")]
    public async Task<IEnumerable<UserInChatResponse>> GetUsersInChat([FromRoute] Guid chatId)
    {
        return _mapper.Map<IEnumerable<UserInChatResponse>>(await _messengerService.GetUsersInChat(chatId));
    }
    
    [HttpPost("messenger/{messageId}/upload")]
    public async Task<IEnumerable<AttachmentResponse>> AddAttachments([FromRoute] Guid messageId)
    {
        Console.WriteLine("LOH: " + Request.Form.Files);
        
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var createdFiles = await _attachmentService.UploadFiles(userId, new AttachmentModelRequest()
        {
            MessageId = messageId,
            FileType = FileType.Message,
            Files = Request.Form.Files
        });

        return _mapper.Map<IEnumerable<AttachmentResponse>>(createdFiles);
    }

    [HttpGet("messenger/{messageId}")]
    public async Task<IEnumerable<AttachmentViewResponse>> GetAttachments([FromRoute] Guid messageId)
    {
        return _mapper.Map<IEnumerable<AttachmentViewResponse>>(
            await _attachmentService.GetAttachments(FileType.Message, messageId));
    }
}