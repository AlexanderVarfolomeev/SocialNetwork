namespace SocialNetwork.Web.Pages.Friends.Models;

public class FriendShipRequest
{
    public Guid Id { get; set; }
    public Guid FromUserId { get; set; }
    public DateTime CreationDateTime { get; set; }
}