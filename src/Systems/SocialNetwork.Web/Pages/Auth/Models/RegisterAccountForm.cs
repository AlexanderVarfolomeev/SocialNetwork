using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Web.Pages.Auth.Models;

public class RegisterAccountForm
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Surname { get; set; }

    [Required]
    public DateTime? Birthdate { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    [MaxLength(16)]
    public string Password { get; set; }

    [Required]
    [Compare(nameof(Password))]
    public string Password2 { get; set; }

    public string PhoneNumber { get; set; } = String.Empty;
}