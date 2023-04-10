using SocialNetwork.Web.Pages.Posts.Models;

namespace SocialNetwork.Web.Pages.Posts.Services;

public interface IPostService
{
    Task<IEnumerable<PostModel>> GetPostsByUser(Guid userId, int offset = 0, int limit = 10);
    Task<IEnumerable<PostModel>> GetAllPosts(int offset = 0, int limit = 10);
    Task<bool> IsUserLikedPost(Guid postId, Guid userId);
    Task LikePost(Guid postId);

    Task<IEnumerable<CommentModel>> GetCommentsByPost(Guid postId);
}