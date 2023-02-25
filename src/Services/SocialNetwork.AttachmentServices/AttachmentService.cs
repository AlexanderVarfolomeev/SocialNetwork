using AutoMapper;
using SocialNetwork.AttachmentServices.Models;
using SocialNetwork.Common.Enum;
using SocialNetwork.Common.Exceptions;
using SocialNetwork.Common.Extensions;
using SocialNetwork.Constants.Errors;
using SocialNetwork.Entities.Files;
using SocialNetwork.Entities.Messenger;
using SocialNetwork.Entities.Posts;
using SocialNetwork.Entities.User;
using SocialNetwork.Repository;

namespace SocialNetwork.AttachmentServices;

public class AttachmentService : IAttachmentService
{
    private readonly IRepository<Post> _postRepository;
    private readonly IRepository<AppUser> _userRepository;
    private readonly IRepository<Comment> _commentRepository;
    private readonly IRepository<Message> _messageRepository;
    private readonly IMapper _mapper;
    private readonly IRepository<Attachment> _attachmentsRepository;

    public AttachmentService(IRepository<Post> postRepository, IRepository<AppUser> userRepository,
        IRepository<Comment> commentRepository, IRepository<Message> messageRepository, IMapper mapper,
        IRepository<Attachment> attachmentsRepository)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
        _commentRepository = commentRepository;
        _messageRepository = messageRepository;
        _mapper = mapper;
        _attachmentsRepository = attachmentsRepository;
    }

    public async Task<IEnumerable<AttachmentModel>> UploadFiles(Guid userId, AttachmentModelRequest attachments)
    {
        bool isCreator = await IsCreatorOfContent(userId, attachments);
        ProcessException.ThrowIf(() => !isCreator, ErrorMessages.OnlyAccountOwnerCanDoIdError);
        ProcessException.ThrowIf(() => attachments.FileType == FileType.Avatar && attachments.Files.Count() > 1,
            ErrorMessages.MorThanOneAvatarError);


        ProcessException.ThrowIf(() => attachments.Files.Count() > 10, ErrorMessages.InvestmentLimitExceededError);

        Directory.CreateDirectory("./wwwroot/Avatar");
        Directory.CreateDirectory("./wwwroot/Message");
        Directory.CreateDirectory("./wwwroot/Post");
        Directory.CreateDirectory("./wwwroot/Comment");

        return await CreateFiles(attachments);
    }

    /// <summary>
    /// Получение вложений в формате base64 строки
    /// </summary>
    /// <param name="type"> Тип вложения: аватар, комментарий, пост, сообщение</param>
    /// <param name="contentId"> Id сущности к которой прикреплены файлы </param>
    public async Task<IEnumerable<AttachmentViewModel>> GetAttachments(FileType type, Guid contentId) => type switch
    {
        //FileType.Avatar => await GetAvatars(contentId),
        FileType.Comment => await GetCommentAttachments(contentId),
        FileType.Message => await GetMessageAttachments(contentId),
        FileType.Post => await GetPostAttachments(contentId),
        _ => throw new ProcessException(ErrorMessages.UnsupportedTypeError)
    };

    public async Task<IEnumerable<AvatarModel>> GetAvatars(Guid userId)
    {
        var avatars =
            await _attachmentsRepository.GetAllAsync(x => x.FileType == FileType.Avatar && x.UserId == userId);
        var avatarsModel = _mapper.Map<IEnumerable<AvatarModel>>(avatars).ToList();
        
        var files = ConvertFilesToBase64(avatars).ToList();
        foreach (var model in avatarsModel)
        {
            model.Content = files.Where(x => x.Id == model.Id).Select(x => x.Content).First();
        }

        return avatarsModel;
    }

    public async Task DeletePostAttachment(Guid userId, Guid postId, Guid attachmentId)
    {
        var attachment = await _attachmentsRepository.GetAsync(attachmentId);
        var post = await _postRepository.GetAsync(postId);
        ProcessException.ThrowIf(() => userId != post.CreatorId, ErrorMessages.OnlyAccountOwnerCanDoIdError);

        await DeleteFile(attachment);
    }

    public async Task DeleteAvatar(Guid userId, Guid avatarId)
    {
        var attachment = await _attachmentsRepository.GetAsync(avatarId);
        ProcessException.ThrowIf(() => userId != attachment.UserId, ErrorMessages.OnlyAccountOwnerCanDoIdError);

        await DeleteFile(attachment);
    }

    /// <summary>
    /// Проверка что пользователь который добавляет фото к посту/комменту/сообщению, является создателем этого
    /// TODO тоже убрать? 
    /// </summary>
    private async Task<bool> IsCreatorOfContent(Guid userId, AttachmentModelRequest attachments) =>
        attachments.FileType switch
        {
            FileType.Message => userId ==
                                (await _messageRepository.GetAsync(x => x.Id == attachments.MessageId)).SenderId,
            FileType.Post => userId == (await _postRepository.GetAsync(x => x.Id == attachments.PostId)).CreatorId,
            FileType.Comment => userId ==
                                (await _commentRepository.GetAsync(x => x.Id == attachments.CommentId)).CreatorId,
            _ => true
        };

    /// <summary>
    /// Создание файлов в файловой системе и сохранение путей к ним в бд
    /// </summary>
    private async Task<IEnumerable<AttachmentModel>> CreateFiles(AttachmentModelRequest attachments)
    {
        var createdFiles = new List<AttachmentModel>();

        // Cоздаем файл в файловой системе и сохраняем путь к нему в бд
        foreach (var file in attachments.Files)
        {
            Attachment fileModel = new Attachment { FileType = attachments.FileType, Name = Path.GetRandomFileName(), };

            switch (attachments.FileType)
            {
                case FileType.Avatar:
                    await ChangeAvatarStatus(attachments.UserId);
                    fileModel.UserId = attachments.UserId;
                    fileModel.IsCurrentAvatar = true;
                    break;
                case FileType.Comment:
                    fileModel.CommentId = attachments.CommentId;
                    break;
                case FileType.Message:
                    fileModel.MessageId = attachments.MessageId;
                    break;
                case FileType.Post:
                    fileModel.PostId = attachments.PostId;
                    break;
            }

            var createdFile = await _attachmentsRepository.AddAsync(fileModel);
            createdFiles.Add(_mapper.Map<AttachmentModel>(createdFile));

            var filePath = Path.Combine(fileModel.FileType.GetPath(), fileModel.Name);
            await using var stream = File.Create(filePath);
            await file.CopyToAsync(stream);
        }

        return createdFiles;
    }

    /// <summary>
    /// Смена статуса текущего аватара
    /// </summary>
    private async Task ChangeAvatarStatus(Guid? userId)
    {
        var avatars = (await _attachmentsRepository.GetAllAsync(x => x.UserId == userId && x.IsCurrentAvatar == true))
            .ToList();

        if (avatars.Count != 0)
        {
            avatars.First().IsCurrentAvatar = false;
            await _attachmentsRepository.UpdateAsync(avatars.First());
        }
    }

    private IEnumerable<AttachmentViewModel> ConvertFilesToBase64(IEnumerable<Attachment> files)
    {
        var result = new List<AttachmentViewModel>();
        foreach (var file in files)
        {
            var path = Path.Combine(file.FileType.GetPath(), file.Name);
            var bytes = File.ReadAllBytes(path);

            AttachmentViewModel viewModel = new AttachmentViewModel
            {
                Content = Convert.ToBase64String(bytes),
                Id = file.Id
            };

            result.Add(viewModel);
        }

        return result;
    }

    /*private async Task<IEnumerable<AttachmentViewModel>> GetAvatars(Guid userId)
    {
        var avatars =
            await _attachmentsRepository.GetAllAsync(x => x.FileType == FileType.Avatar && x.UserId == userId);
        return ConvertFilesToBase64(avatars);
    }*/

    private async Task<IEnumerable<AttachmentViewModel>> GetCommentAttachments(Guid commentId)
    {
        var attachments =
            await _attachmentsRepository.GetAllAsync(x => x.FileType == FileType.Comment && x.CommentId == commentId);
        return ConvertFilesToBase64(attachments);
    }

    private async Task<IEnumerable<AttachmentViewModel>> GetMessageAttachments(Guid messageId)
    {
        var attachments =
            await _attachmentsRepository.GetAllAsync(x => x.FileType == FileType.Message && x.MessageId == messageId);
        return ConvertFilesToBase64(attachments);
    }

    private async Task<IEnumerable<AttachmentViewModel>> GetPostAttachments(Guid postId)
    {
        var attachments =
            await _attachmentsRepository.GetAllAsync(x => x.FileType == FileType.Post && x.PostId == postId);
        return ConvertFilesToBase64(attachments);
    }

    private async Task DeleteFile(Attachment attachment)
    {
        var pathToFile = Path.Combine(attachment.FileType.GetPath(), attachment.Name);
        File.Delete(pathToFile); // Удаляем файл с системы
        await _attachmentsRepository.DeleteAsync(attachment); // Удаляем файл с бд
    }
}