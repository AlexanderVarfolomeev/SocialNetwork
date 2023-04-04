namespace SocialNetwork.Web.Pages.Users.Models;

public class AvatarModel
{
    public Guid? UserId { get; set; }
    public bool IsCurrentAvatar { get; set; } = false;
    public string Content { get; set; }
}