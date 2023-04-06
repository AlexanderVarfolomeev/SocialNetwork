using AutoMapper;
using SocialNetwork.Entities.Base;
using SocialNetwork.Entities.Messenger;

namespace SocialNetwork.MessengerService.Models;

public class UserInChatModelResponse : BaseEntity
{
    public bool IsCreator { get; set; }

    public DateTime EntryDate { get; set; }

    public Guid ChatId { get; set; }

    public Guid UserId { get; set; }
}

public class UserInChatModelResponseProfile : Profile
{
    public UserInChatModelResponseProfile()
    {
        CreateMap<UserInChat, UserInChatModelResponse>();
    }
}