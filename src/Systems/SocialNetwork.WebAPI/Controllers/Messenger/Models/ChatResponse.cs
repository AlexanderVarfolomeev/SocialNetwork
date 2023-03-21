using AutoMapper;
using SocialNetwork.Entities.Base;
using SocialNetwork.MessengerService.Models;

namespace SocialNetwork.WebAPI.Controllers.Messenger.Models;

public class ChatResponse : BaseEntity
{
    public bool IsDialog { get; set; }
    public string ChatName { get; set; }
}

public class ChatResponseProfile : Profile
{
    public ChatResponseProfile()
    {
        CreateMap<ChatModelResponse, ChatResponse>();
    }
}