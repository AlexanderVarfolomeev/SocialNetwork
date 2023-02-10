using SocialNetwork.AccountServices.Models;
using SocialNetwork.RelationshipServices.Models;

namespace SocialNetwork.RelationshipServices;

public interface IRelationshipService
{
    Task SendFriendRequest(Guid senderId, Guid recipientId);
    Task AcceptFriendRequest(Guid recipientId, Guid requestId);
    Task RejectFriendRequest(Guid recipientId, Guid requestId);
    Task<List<FriendshipRequest>> GetFriendshipRequests(Guid userId, int offset = 0, int limit = 10);
    Task<List<AppAccountModel>> GetFriendList(Guid userId, int offset = 0, int limit = 10);
    Task DeleteFromFriendList(Guid userId, Guid friendId);
}