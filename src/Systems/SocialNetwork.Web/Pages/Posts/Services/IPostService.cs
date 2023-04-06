using System.Collections;
using SocialNetwork.Web.Pages.Posts.Models;

namespace SocialNetwork.Web.Pages.Posts.Services;

public interface IPostService
{
    Task<IEnumerable<PostModel>> GetPostsByUser(Guid userId, int offset = 0, int limit = 10);
    Task<IEnumerable<AttachmentModel>> GetAttachments(Guid postId);
}