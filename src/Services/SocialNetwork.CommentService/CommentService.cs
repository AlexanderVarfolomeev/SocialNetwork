using AutoMapper;
using SocialNetwork.CommentService.Models;
using SocialNetwork.Common.Exceptions;
using SocialNetwork.Constants.Errors;
using SocialNetwork.Entities.Posts;
using SocialNetwork.Entities.User;
using SocialNetwork.Repository;

namespace SocialNetwork.CommentService;

public class CommentService : ICommentService
{
    private readonly IMapper _mapper;
    private readonly IRepository<Comment> _commentsRepository;
    private readonly IRepository<AppUser> _usersRepository;
    private readonly IRepository<Post> _postRepository;

    public CommentService(
        IMapper mapper,
        IRepository<Comment> commentsRepository,
        IRepository<AppUser> usersRepository,
        IRepository<Post> postRepository)
    {
        _mapper = mapper;
        _commentsRepository = commentsRepository;
        _usersRepository = usersRepository;
        _postRepository = postRepository;
    }

    public async Task<IEnumerable<CommentModelResponse>> GetCommentsByPost(Guid postId, int offset = 0, int limit = 10)
    {
        var posts = await _commentsRepository.GetAllAsync(x => x.PostId == postId, offset, limit);
        return _mapper.Map<IEnumerable<CommentModelResponse>>(posts);
    }

    public async Task<CommentModelResponse> AddComment(Guid userId, Guid postId, CommentModelRequest commentModel)
    {
        var user = await _usersRepository.GetAsync(userId);
        ProcessException.ThrowIf(() => user.IsBanned, ErrorMessages.YouBannedError);
        await _postRepository.GetAsync(postId);
        
        var createdPost = await _commentsRepository.AddAsync(new Comment()
        {
            PostId = postId,
            CreatorId = userId,
            Text = commentModel.Text
        });
        
        return _mapper.Map<CommentModelResponse>(createdPost);
    }

    public async Task<CommentModelResponse> UpdateComment(Guid userId, Guid commentId, CommentModelRequest commentModel)
    {
        var user = await _usersRepository.GetAsync(userId);
        ProcessException.ThrowIf(() => user.IsBanned, ErrorMessages.YouBannedError);
        var comment = await _commentsRepository.GetAsync(commentId);
        ProcessException.ThrowIf(() => comment.CreatorId != userId, ErrorMessages.OnlyCreatorOfContentCanDoItError);
        comment.Text = commentModel.Text;
        return _mapper.Map<CommentModelResponse>(await _commentsRepository.UpdateAsync(comment));
    }

    public async Task DeleteComment(Guid userId, Guid commentId)
    {
        var user = await _usersRepository.GetAsync(userId);
        ProcessException.ThrowIf(() => user.IsBanned, ErrorMessages.YouBannedError);
        var comment = await _commentsRepository.GetAsync(commentId);
        ProcessException.ThrowIf(() => comment.CreatorId != userId, ErrorMessages.OnlyCreatorOfContentCanDoItError);

        await _commentsRepository.DeleteAsync(comment);
    }
}