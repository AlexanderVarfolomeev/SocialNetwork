using AutoMapper;
using SocialNetwork.Entities.Base;
using SocialNetwork.Entities.Messenger;

namespace SocialNetwork.MessengerService.Models;

public class ChatModelResponse : BaseEntity
{
    public bool IsDialog { get; set; }
    public string ChatName { get; set; }
}

public class ChatModelResponseProfile : Profile
{
    public ChatModelResponseProfile()
    {
        CreateMap<Chat, ChatModelResponse>();
    }
}