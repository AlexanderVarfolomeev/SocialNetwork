namespace SocialNetwork.Web.Pages.Posts.Models;

public class PostModel
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsInGroup { get; set; }

    public Guid CreatorId { get; set; }
    public Guid? GroupId { get; set; }
    
    public IEnumerable<string> Attachments { get; set; }
}