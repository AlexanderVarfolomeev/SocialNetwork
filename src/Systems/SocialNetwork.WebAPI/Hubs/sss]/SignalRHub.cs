using Microsoft.AspNetCore.SignalR;
using SocialNetwork.WebAPI.Controllers.Messenger.Models;

namespace SocialNetwork.WebAPI.Hubs.sss_;

public class SignalRHub : Hub
{
    public async Task SendMessageAsync(MessageResponse message, string userName)
    {
        await Clients.All.SendAsync("ReceiveMessage", message, userName);
    }
}