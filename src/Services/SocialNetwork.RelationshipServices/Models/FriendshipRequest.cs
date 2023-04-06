using AutoMapper;
using SocialNetwork.Entities.User;

namespace SocialNetwork.RelationshipServices.Models;

public class FriendshipRequest
{
    public Guid Id { get; set; }
    public Guid FromUserId { get; set; }
    public DateTime CreationDateTime { get; set; }
}

public class FriendshipRequestProfile : Profile
{
    public FriendshipRequestProfile()
    {
        CreateMap<Relationship, FriendshipRequest>()
            .ForMember(d => d.CreationDateTime, opts => opts.MapFrom(s => s.CreationDateTime))
            .ForMember(d => d.FromUserId, opts => opts.MapFrom(s => s.FirstUserId));
    }
}