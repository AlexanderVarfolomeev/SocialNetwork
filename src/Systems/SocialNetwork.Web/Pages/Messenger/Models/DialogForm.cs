namespace SocialNetwork.Web.Pages.Messenger.Models;

public class DialogForm
{
    public string Username { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public Guid DialogId { get; set; }
    public string Avatar { get; set; }
}