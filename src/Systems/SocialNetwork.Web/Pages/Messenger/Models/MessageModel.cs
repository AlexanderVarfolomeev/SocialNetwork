using SocialNetwork.Web.Pages.Posts.Models;
using SocialNetwork.Web.Pages.Users.Models;

namespace SocialNetwork.Web.Pages.Messenger.Models;

public class MessageModel
{
    public Guid Id { get; set; }
    public DateTime CreationDateTime { get; set; }
    
    public string Text { get; set; }
    public bool IsRead { get; set; }
    public bool IsModification { get; set; }

    public Guid SenderId { get; set; }
    public Guid ChatId { get; set; }
    
    public AccountModel User { get; set; }
    public List<AttachmentModel> Attachments { get; set; }
}