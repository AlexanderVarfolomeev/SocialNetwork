using SocialNetwork.PostServices.Models;

namespace SocialNetwork.PostServices;

public interface IPostService
{
    Task<IEnumerable<PostModelResponse>> GetPosts(int offset = 0, int limit = 10);
    Task<IEnumerable<PostModelResponse>> GetUsersPosts(Guid userId, int offset = 0, int limit = 10);
    Task<IEnumerable<PostModelResponse>> GetGroupPosts(Guid groupId, int offset = 0, int limit = 10);
    Task<PostModelResponse> AddUserPost(Guid userId, PostModelAdd postModel);
    Task<PostModelResponse> AddGroupPost(Guid userId, PostModelAdd postModel);
    Task<PostModelResponse> UpdatePost(Guid userId, Guid postId, PostModelUpdate postModel);
    Task DeletePost(Guid userId, Guid postId);
}