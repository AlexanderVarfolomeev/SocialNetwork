using SocialNetwork.AccountServices.Models;
using SocialNetwork.RelationshipServices.Models;

namespace SocialNetwork.RelationshipServices;

public interface IRelationshipService
{
    Task SendFriendRequest(Guid userId);
    Task AcceptFriendRequest(Guid requestId);
    Task RejectFriendRequest(Guid requestId);
    Task<List<FriendshipRequest>> GetFriendshipRequests(int offset = 0, int limit = 10);
    Task<List<AppAccountModel>> GetFriendList(int offset = 0, int limit = 10);
    Task DeleteFromFriendList(Guid userId);
}