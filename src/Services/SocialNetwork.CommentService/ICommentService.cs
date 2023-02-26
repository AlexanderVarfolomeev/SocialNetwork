using SocialNetwork.CommentService.Models;

namespace SocialNetwork.CommentService;

public interface ICommentService
{
    Task<IEnumerable<CommentModelResponse>> GetCommentsByPost(Guid postId, int offset = 0, int limit = 10);
    Task<CommentModelResponse> AddComment(Guid userId, Guid postId, CommentModelRequest commentModel);
    Task<CommentModelResponse> UpdateComment(Guid userId, Guid commentId, CommentModelRequest commentModel);
    Task DeleteComment(Guid userId, Guid commentId);
}