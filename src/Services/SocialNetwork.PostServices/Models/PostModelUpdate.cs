using SocialNetwork.Entities.Base;

namespace SocialNetwork.PostServices.Models;

public class PostModelUpdate : BaseEntity
{
    public string Text { get; set; } = string.Empty;
}