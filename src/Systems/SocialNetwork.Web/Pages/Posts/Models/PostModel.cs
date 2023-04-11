using SocialNetwork.Web.Pages.Users.Models;

namespace SocialNetwork.Web.Pages.Posts.Models;

public class PostModel
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsInGroup { get; set; }

    public Guid CreatorId { get; set; }
    public Guid? GroupId { get; set; }
    
    public DateTime CreationDateTime { get; set; }
    
    public IEnumerable<AttachmentModel> Attachments { get; set; }
    public AccountModel Creator { get; set; }
    public int Likes { get; set; }
    public IEnumerable<CommentModel> Comments { get; set; }
    public bool IsLiked { get; set; } = false;
}