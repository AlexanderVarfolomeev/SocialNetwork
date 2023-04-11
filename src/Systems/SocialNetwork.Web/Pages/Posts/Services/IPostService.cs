using Microsoft.AspNetCore.Components.Forms;
using SocialNetwork.Web.Pages.Posts.Models;

namespace SocialNetwork.Web.Pages.Posts.Services;

public interface IPostService
{
    Task<IEnumerable<PostModel>> GetPostsByUser(Guid userId, int offset = 0, int limit = 10);
    Task<IEnumerable<PostModel>> GetAllPosts(int offset = 0, int limit = 10);
    Task<bool> IsUserLikedPost(Guid postId);
    Task LikePost(Guid postId);
    Task<IEnumerable<CommentModel>> GetCommentsByPost(Guid postId, int offset=0, int limit=1000);
    Task<PostModel> AddPost(PostAddModel post);
    Task AddAttachments(Guid postId, List<IBrowserFile> files);
    Task<IEnumerable<AttachmentModel>> GetPostAttachments(Guid postId);
}