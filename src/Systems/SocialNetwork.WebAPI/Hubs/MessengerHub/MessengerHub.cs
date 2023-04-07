using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SocialNetwork.AccountServices.Interfaces;
using SocialNetwork.Common.Exceptions;
using SocialNetwork.MessengerService;
using SocialNetwork.MessengerService.Models;
using SocialNetwork.WebAPI.Controllers.Messenger.Models;
using SocialNetwork.WebAPI.Hubs.Models;

namespace SocialNetwork.WebAPI.Hubs.MessengerHub;


[Authorize]
public class MessengerHub : Hub
{
    private readonly IAccountService _accountService;
    private readonly IMapper _mapper;
    private readonly IMessengerService _messengerService;
    private static readonly ConnectionList ConnectionList = new();

    public MessengerHub(IAccountService accountService, IMapper mapper, IMessengerService messengerService)
    {
        _accountService = accountService;
        _mapper = mapper;
        _messengerService = messengerService;
    }


    public override async Task OnConnectedAsync()
    {
        
        var a = Context.User.FindFirst("nickname");
        var user = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //Console.WriteLine(userId);
        var currentUserName = (await _accountService.GetAccountAsync(Guid.Parse(user))).UserName;
        var connectionId = Context.ConnectionId;

        ConnectionList.Add(currentUserName, connectionId);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var user = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var currentUserName = (await _accountService.GetAccountAsync(Guid.Parse(user))).UserName;
        var connectionId = Context.ConnectionId;

        ConnectionList.Remove(currentUserName, connectionId);
        await base.OnDisconnectedAsync(exception);
    }


    public async Task SendMessage(string message, Guid userId)
    {
        Console.WriteLine("BBBBBBBBBBBBBBBBBBBBBB");
        Console.WriteLine(message);
        Console.WriteLine(userId);
        
        var senderId = Guid.Parse(Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        Console.WriteLine(senderId);
        
        ProcessException.ThrowIf(() => senderId == userId, "You can't send a message to yourself");

        var senderUserName = (await _accountService.GetAccountAsync(senderId)).UserName;

        var userUserName = (await _accountService.GetAccountAsync(userId)).UserName;

        var connections = new List<string>();

        foreach (var connection in ConnectionList.GetConnections(senderUserName))
        {
            connections.Add(connection);
        }

        foreach (var connection in ConnectionList.GetConnections(userUserName))
        {
            connections.Add(connection);
        }

        MessageResponse response =
            _mapper.Map<MessageResponse>(await _messengerService.SendMessageToUser(senderId, userId, new MessageModelRequest()
            {
                Text = message
            }));
        Console.WriteLine(response.Text);
        Console.WriteLine(Clients.Clients(connections));
        await Clients.Clients(connections)
            .SendAsync("ReceiveMessage",
                response); // При получении на клиенте мы сразу вызовем запрос на получение картинок
    }
}