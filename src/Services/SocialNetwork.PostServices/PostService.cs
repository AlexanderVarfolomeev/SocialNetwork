using AutoMapper;
using SocialNetwork.Common.Exceptions;
using SocialNetwork.Constants.Errors;
using SocialNetwork.Entities.Groups;
using SocialNetwork.Entities.Posts;
using SocialNetwork.Entities.User;
using SocialNetwork.PostServices.Models;
using SocialNetwork.Repository;

namespace SocialNetwork.PostServices;

public class PostService : IPostService
{
    private readonly IRepository<Post> _postRepository;
    private readonly IRepository<Group> _groupRepository;
    private readonly IMapper _mapper;
    private readonly IRepository<AppUser> _userRepository;
    private readonly IRepository<UserLikePost> _likesRepository;

    public PostService(
        IRepository<Post> postRepository,
        IRepository<Group> groupRepository,
        IMapper mapper,
        IRepository<AppUser> userRepository,
        IRepository<UserLikePost> likesRepository)
    {
        _postRepository = postRepository;
        _groupRepository = groupRepository;
        _mapper = mapper;
        _userRepository = userRepository;
        _likesRepository = likesRepository;
    }

    public async Task<IEnumerable<PostModelResponse>> GetPosts(int offset = 0, int limit = 10)
    {
        return _mapper.Map<IEnumerable<PostModelResponse>>(
                await _postRepository.GetAllAsync(offset, limit))
            .OrderBy(x => x.CreationDateTime).ToList();
    }

    public async Task<IEnumerable<PostModelResponse>> GetUsersPosts(Guid userId, int offset = 0, int limit = 10)
    {
        try
        {
            await _userRepository.GetAsync(userId);
            return _mapper.Map<IEnumerable<PostModelResponse>>(
                    await _postRepository.GetAllAsync((x) => x.CreatorId == userId, offset, limit))
                .OrderBy(x => x.CreationDateTime).ToList();
        }
        catch (ProcessException e)
        {
            throw new ProcessException(ErrorMessages.UserNotFoundError, e);
        }
    }

    public async Task<IEnumerable<PostModelResponse>> GetUsersPostsByName(string username, int offset = 0,
        int limit = 10)
    {
        var user = await _userRepository.GetAsync(x => x.UserName == username);
        return await GetUsersPosts(user.Id, offset, limit);
    }

    public async Task<IEnumerable<PostModelResponse>> GetGroupPosts(Guid groupId, int offset = 0, int limit = 10)
    {
        try
        {
            await _groupRepository.GetAsync(groupId);
            return _mapper.Map<IEnumerable<PostModelResponse>>(
                    await _postRepository.GetAllAsync((x) => x.GroupId == groupId, offset, limit))
                .OrderBy(x => x.CreationDateTime).ToList();
        }
        catch (ProcessException e)
        {
            throw new ProcessException(ErrorMessages.GroupNotFoundError, e);
        }
    }

    public async Task<PostModelResponse> AddUserPost(Guid userId, PostModelAdd postModel)
    {
        var user = await _userRepository.GetAsync(userId);
        ProcessException.ThrowIf(() => user.IsBanned, ErrorMessages.YouBannedError);

        var createdPost = await _postRepository.AddAsync(_mapper.Map<Post>(postModel));
        return _mapper.Map<PostModelResponse>(createdPost);
    }

    public async Task<PostModelResponse> AddGroupPost(Guid userId, PostModelAdd postModel)
    {
        var user = await _userRepository.GetAsync(userId);
        var group = await _groupRepository.GetAsync((Guid)postModel.GroupId!);
        var groupAdmins = group.Users.Where(x => x.IsAdmin).Select(x => x.UserId);

        ProcessException.ThrowIf(() => user.IsBanned, ErrorMessages.YouBannedError);
        ProcessException.ThrowIf(() => !groupAdmins.Contains(userId),
            ErrorMessages.OnlyAdminOfGroupCanAddContentInError);


        var createdPost = await _postRepository.AddAsync(_mapper.Map<Post>(postModel));
        return _mapper.Map<PostModelResponse>(createdPost);
    }

    public async Task<PostModelResponse> UpdatePost(Guid userId, Guid postId, PostModelUpdate postModel)
    {
        var post = await GetPostIfUserCreator(userId, postId);

        post.Text = postModel.Text;
        return _mapper.Map<PostModelResponse>(await _postRepository.UpdateAsync(post));
    }

    public async Task DeletePost(Guid userId, Guid postId)
    {
        var post = await GetPostIfUserCreator(userId, postId);
        await _postRepository.DeleteAsync(post);
    }

    public async Task LikePost(Guid userId, Guid postId)
    {
        var user = await _userRepository.GetAsync(userId);
        ProcessException.ThrowIf(() => user.IsBanned, ErrorMessages.YouBannedError);

        var likes = (await _likesRepository.GetAllAsync(x => x.PostId == postId && x.UserId == userId)).ToList();

        // Если пользователь уже лайкал пост, то убираем лайк, иначе добавляем
        if (likes.Any())
            await _likesRepository.DeleteAsync(likes.First());
        else
            await _likesRepository.AddAsync(new UserLikePost() { UserId = userId, PostId = postId });
    }

    public async Task<int> GetCountOfLikes(Guid postId)
    {
        return (await _likesRepository.GetAllAsync(x => x.PostId == postId)).Count();
    }

    /// <summary>
    /// Проверка что пользователь создатель поста и админ в группе в которой пост создан
    /// </summary>
    private async Task<Post> GetPostIfUserCreator(Guid userId, Guid postId)
    {
        var user = await _userRepository.GetAsync(userId);
        var post = await _postRepository.GetAsync(postId);
        ProcessException.ThrowIf(() => user.IsBanned, ErrorMessages.YouBannedError);
        ProcessException.ThrowIf(() => post.CreatorId != userId, ErrorMessages.OnlyCreatorOfContentCanDoIt);

        if (post.IsInGroup)
        {
            // Если пост выложен в группе, проверяем что пользователь все еще админ в ней
            var group = await _groupRepository.GetAsync((Guid)post.GroupId!);
            var groupAdmins = group.Users.Where(x => x.IsAdmin).Select(x => x.UserId);
            ProcessException.ThrowIf(() => !groupAdmins.Contains(userId),
                ErrorMessages.OnlyAdminOfGroupCanAddContentInError);
        }

        return post;
    }
}