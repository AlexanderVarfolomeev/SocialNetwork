namespace SocialNetwork.Web.Pages.Posts.Models;

public class PostModel
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public string UserName { get; set; }
    public bool IsInGroup { get; set; }

    public Guid CreatorId { get; set; }
    public Guid? GroupId { get; set; }
    
    public DateTime CreationDateTime { get; set; }
    
    public IEnumerable<string> Attachments { get; set; }
    public string UserAvatar { get; set; }
}