using System.Diagnostics;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using SocialNetwork.AttachmentServices.Models;
using SocialNetwork.Common.Enum;
using SocialNetwork.Common.Exceptions;
using SocialNetwork.Common.Extensions;
using SocialNetwork.Constants.Errors;
using SocialNetwork.Entities.Base;
using SocialNetwork.Entities.Files;
using SocialNetwork.Entities.Messenger;
using SocialNetwork.Entities.Posts;
using SocialNetwork.Entities.User;
using SocialNetwork.Repository;

namespace SocialNetwork.AttachmentServices;

// TODO deleteFile, getFiles
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

        return await CreateFiles(attachments);
    }

    public async Task<IEnumerable<string>> GetAvatars(Guid userId)
    {
        var avatars =
            await _attachmentsRepository.GetAllAsync(x => x.FileType == FileType.Avatar && x.UserId == userId);
        return ConvertFilesToBase64(avatars);
    }

    public async Task<string> GetCurrentAvatar(Guid userId)
    {
        var avatar = await _attachmentsRepository.GetAsync(x =>
            x.FileType == FileType.Avatar
            && x.UserId == userId
            && x.IsCurrentAvatar == true);

        return ConvertFilesToBase64(new[] { avatar }).First();
    }

    /// <summary>
    /// Проверка что пользователь который добавляет фото к посту/комменту/сообщению, является создателем этого
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

            createdFiles.Add(_mapper.Map<AttachmentModel>(fileModel));
            await _attachmentsRepository.AddAsync(fileModel);

            var filePath = Path.Combine(fileModel.FileType.GetPath(), fileModel.Name);
            await using var stream = File.Create(filePath);
            await file.CopyToAsync(stream);
        }

        return createdFiles;
    }

    /// <summary>
    /// Смена статуса текущего аватара
    /// </summary>
    /// <param name="userId"></param>
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

    private IEnumerable<string> ConvertFilesToBase64(IEnumerable<Attachment> files)
    {
        var result = new List<string>();
        foreach (var file in files)
        {
            var path = Path.Combine(file.FileType.GetPath(), file.Name);
            var bytes = File.ReadAllBytes(path);
            result.Add(Convert.ToBase64String(bytes));
        }

        return result;
    }
}