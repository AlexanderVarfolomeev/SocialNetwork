using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.MessengerService;
using SocialNetwork.MessengerService.Models;
using SocialNetwork.WebAPI.Controllers.Messenger.Models;

namespace SocialNetwork.WebAPI.Controllers.Messenger;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/")]
[ApiController]
public class MessengerController : ControllerBase
{
    private readonly IMessengerService _messengerService;
    private readonly IMapper _mapper;

    public MessengerController(IMessengerService messengerService, IMapper mapper)
    {
        _messengerService = messengerService;
        _mapper = mapper;
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
    public async Task<IEnumerable<MessageResponse>> GetMessages([FromRoute] Guid chatId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        return _mapper.Map<IEnumerable<MessageResponse>>(await _messengerService.GetMessages(userId, chatId));
    }
}