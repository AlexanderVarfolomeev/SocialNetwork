namespace SocialNetwork.Web.Pages.Users.Models;

public class AccountModel
{
    public Guid Id { get; set; }
    public DateTimeOffset CreationDateTime { get; set; }
    public DateTimeOffset ModificationDateTime { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;

    public DateTimeOffset Birthday { get; set; }
    public string Status { get; set; } = string.Empty;

    public bool IsBanned { get; set; }
    
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}