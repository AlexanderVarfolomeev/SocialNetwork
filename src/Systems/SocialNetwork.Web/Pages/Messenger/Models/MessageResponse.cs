namespace SocialNetwork.Web.Pages.Messenger.Models;

public class MessageResponse
{
    public Guid Id { get; set; }
    public DateTime CreationDateTime { get; set; }
    public DateTime ModificationDateTime { get; set; }
    public string Text { get; set; }
    public bool IsRead { get; set; }
    public bool IsModification { get; set; }
    public Guid SenderId { get; set; }
    public Guid ChatId { get; set; }
}