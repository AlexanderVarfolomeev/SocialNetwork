namespace SocialNetwork.Web.Pages.Messenger.Models;

public class UserInChatModel
{
    public Guid Id { get; set; }
    public bool IsCreator { get; set; }

    public DateTime EntryDate { get; set; }

    public Guid ChatId { get; set; }

    public Guid UserId { get; set; }
}