using SocialNetwork.Web.Pages.Users.Models;

namespace SocialNetwork.Web.Pages.Posts.Models;

public class CommentModel
{
    public Guid Id { get; set; }
    public DateTime CreationDateTime { get; set; }
    public DateTime ModificationDateTime { get; set; }
    public string Text { get; set; }
    public Guid CreatorId { get; set; }
    public Guid PostId { get; set; }
    
    public AccountModel Creator { get; set; }
    public IEnumerable<AttachmentModel> Attachments { get; set; }
}