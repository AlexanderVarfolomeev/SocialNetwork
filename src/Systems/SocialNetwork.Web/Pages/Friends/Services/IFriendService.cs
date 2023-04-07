using SocialNetwork.Web.Pages.Friends.Models;

namespace SocialNetwork.Web.Pages.Friends.Services;

public interface IFriendService
{
    Task SendFriendshipRequest(Guid userId);
    Task AcceptFriendshipRequest(Guid requestId);
    Task RejectFriendshipRequest(Guid requestId);
    Task<IEnumerable<FriendShipRequest>> GetFriendshipRequests();
}