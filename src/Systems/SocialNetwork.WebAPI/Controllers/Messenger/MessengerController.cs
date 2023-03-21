using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.MessengerService;
using SocialNetwork.MessengerService.Models;

namespace SocialNetwork.WebAPI.Controllers.Messenger;


[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/")]
[ApiController]
public class MessengerController : ControllerBase
{
    private readonly IMessengerService _messengerService;

    public MessengerController(IMessengerService messengerService)
    {
        _messengerService = messengerService;
    }

    [HttpPost("messenger/{receiverId}")]
    public async Task<MessageModelResponse> SendMessage([FromRoute] Guid receiverId, [FromBody] string text)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var res = await _messengerService.SendMessageToUser(userId, receiverId, new MessageModelRequest(){Text = text});
        return res;
    }
}