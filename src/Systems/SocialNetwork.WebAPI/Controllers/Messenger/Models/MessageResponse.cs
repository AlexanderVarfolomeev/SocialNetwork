using AutoMapper;
using SocialNetwork.Entities.Base;
using SocialNetwork.MessengerService.Models;

namespace SocialNetwork.WebAPI.Controllers.Messenger.Models;

public class MessageResponse : BaseEntity
{
    public string Text { get; set; }
    public bool IsRead { get; set; }
    public bool IsModification { get; set; }

    public Guid SenderId { get; set; }
    public Guid ChatId { get; set; }
}

public class MessageResponseProfile : Profile
{
    public MessageResponseProfile()
    {
        CreateMap<MessageModelResponse, MessageResponse>();
    }
}