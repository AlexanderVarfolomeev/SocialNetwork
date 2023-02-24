using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.AttachmentServices;
using SocialNetwork.AttachmentServices.Models;
using SocialNetwork.Common.Enum;
using SocialNetwork.Constants.Security;
using SocialNetwork.PostServices;
using SocialNetwork.PostServices.Models;
using SocialNetwork.WebAPI.Controllers.CommonModels;
using SocialNetwork.WebAPI.Controllers.Posts.Models;

namespace SocialNetwork.WebAPI.Controllers.Posts;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly IMapper _mapper;
    private readonly IAttachmentService _attachmentService;

    public PostController(IPostService postService, IMapper mapper, IAttachmentService attachmentService)
    {
        _postService = postService;
        _mapper = mapper;
        _attachmentService = attachmentService;
    }

    /// <summary>
    /// Получить все посты
    /// </summary>
    [HttpGet("posts")]
    public async Task<IEnumerable<PostResponse>> GetAllPosts([FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        return _mapper.Map<IEnumerable<PostResponse>>(await _postService.GetPosts(offset, limit));
    }

    /// <summary>
    /// Получить посты пользователя по его Id
    /// </summary>
    [HttpGet("accounts/{userId}/posts")]
    public async Task<IEnumerable<PostResponse>> GetUsersPosts([FromRoute] Guid userId,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 10)
    {
        return _mapper.Map<IEnumerable<PostResponse>>(await _postService.GetUsersPosts(userId, offset, limit));
    }

    /// <summary>
    /// Получить посты пользователя по его никнейму
    /// </summary>
    [HttpGet("accounts/{userName}/posts")]
    public async Task<IEnumerable<PostResponse>> GetUsersPostsByUsername([FromRoute] string userName,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 10)
    {
        return _mapper.Map<IEnumerable<PostResponse>>(await _postService.GetUsersPosts(userName, offset, limit));
    }

    /// <summary>
    /// Получить посты группы
    /// </summary>
    [HttpGet("groups/{groupId}/posts")]
    public async Task<IEnumerable<PostResponse>> GetGroupPosts([FromRoute] Guid groupId,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 10)
    {
        return _mapper.Map<IEnumerable<PostResponse>>(await _postService.GetGroupPosts(groupId, offset, limit));
    }

    /// <summary>
    /// Добавить пост на личный блог
    /// </summary>
    [Authorize(AppScopes.NetworkWrite)]
    [HttpPost("posts")]
    public async Task<PostResponse> AddUsersPost([FromBody] PostRequest post)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var postModelAdd = new PostModelAdd()
        {
            CreatorId = userId,
            GroupId = null,
            IsInGroup = false,
            Text = post.Text
        };

        return _mapper.Map<PostResponse>(await _postService.AddUserPost(userId, postModelAdd));
    }

    /// <summary>
    /// Добавить пост в группу
    /// </summary>
    [Authorize(AppScopes.NetworkWrite)]
    [HttpPost("{groupId}/posts")]
    public async Task<PostResponse> AddGroupPost([FromRoute] Guid groupId, [FromBody] PostRequest post)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var postModelAdd = new PostModelAdd()
        {
            CreatorId = userId,
            GroupId = groupId,
            IsInGroup = true,
            Text = post.Text
        };

        return _mapper.Map<PostResponse>(await _postService.AddGroupPost(userId, postModelAdd));
    }

    /// <summary>
    /// Изменить пост
    /// </summary>
    [Authorize(AppScopes.NetworkWrite)]
    [HttpPatch("posts/{postId}")]
    public async Task<PostResponse> UpdatePost([FromRoute] Guid postId, PostRequest postRequest)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        return _mapper.Map<PostResponse>(await _postService.UpdatePost(userId, postId,
            _mapper.Map<PostModelUpdate>(postRequest)));
    }

    /// <summary>
    /// Удалить пост
    /// </summary>
    [Authorize(AppScopes.NetworkWrite)]
    [HttpDelete("posts/{postId}")]
    public async Task<IActionResult> DeletePost([FromRoute] Guid postId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _postService.DeletePost(userId, postId);
        return Ok();
    }

    /// <summary>
    /// Прикрепить изображение к посту
    /// </summary>
    [Authorize(AppScopes.NetworkWrite)]
    [HttpPost("posts/{postId}")]
    public async Task<IEnumerable<AttachmentResponse>> AddAttachments([FromRoute] Guid postId,
        IEnumerable<IFormFile> attachments)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var createdFiles = await _attachmentService.UploadFiles(userId, new AttachmentModelRequest()
        {
            PostId = postId,
            FileType = FileType.Post,
            Files = attachments
        });

        return _mapper.Map<IEnumerable<AttachmentResponse>>(createdFiles);
    }

    /// <summary>
    /// Получить прикрепленные к посту изображения
    /// </summary>
    [HttpGet("posts/{postId}/attachments")]
    public async Task<IEnumerable<AttachmentViewResponse>> GetAttachments([FromRoute] Guid postId)
    {
        return _mapper.Map<IEnumerable<AttachmentViewResponse>>(
            await _attachmentService.GetAttachments(FileType.Post, postId));
    }

    /// <summary>
    /// Удалить прикрепленное изображение
    /// </summary>
    [Authorize(AppScopes.NetworkWrite)]
    [HttpDelete("posts/{postId}/attachments/{attachmentId}")]
    public async Task<IActionResult> DeleteAttachment([FromRoute] Guid postId, [FromRoute] Guid attachmentId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _attachmentService.DeletePostAttachment(userId, postId, attachmentId);
        return Ok();
    }

    [Authorize(AppScopes.NetworkWrite)]
    [HttpPost("posts/{postId}/likes")]
    public async Task<IActionResult> LikePost([FromRoute] Guid postId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _postService.LikePost(userId, postId);
        return Ok();
    }

    [HttpGet("posts/{postId}/likes")]
    public async Task<int> GetCountOfLikes([FromRoute] Guid postId)
    {
        return await _postService.GetCountOfLikes(postId);
    }
}