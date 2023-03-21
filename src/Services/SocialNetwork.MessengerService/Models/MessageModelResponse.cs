using AutoMapper;
using SocialNetwork.Entities.Base;
using SocialNetwork.Entities.Messenger;

namespace SocialNetwork.MessengerService.Models;

public class MessageModelResponse : BaseEntity
{
    public string Text { get; set; }
    public bool IsRead { get; set; }
    public bool IsModification { get; set; }

    public Guid SenderId { get; set; }
    public Guid ChatId { get; set; }
}

public class MessageModelResponseProfile : Profile
{
    public MessageModelResponseProfile()
    {
        CreateMap<Message, MessageModelResponse>();
    }
}