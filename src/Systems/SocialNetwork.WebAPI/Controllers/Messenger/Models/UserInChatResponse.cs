using AutoMapper;
using SocialNetwork.Entities.Base;
using SocialNetwork.MessengerService.Models;

namespace SocialNetwork.WebAPI.Controllers.Messenger.Models;

public class UserInChatResponse : BaseEntity
{
    public bool IsCreator { get; set; }

    public DateTime EntryDate { get; set; }

    public Guid ChatId { get; set; }

    public Guid UserId { get; set; }
}

public class UserInChatResponseProfile : Profile
{
    public UserInChatResponseProfile()
    {
        CreateMap<UserInChatModelResponse, UserInChatResponse>();
    }
}