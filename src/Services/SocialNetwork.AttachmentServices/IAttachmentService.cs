using SocialNetwork.AttachmentServices.Models;
using SocialNetwork.Common.Enum;

namespace SocialNetwork.AttachmentServices;

/// <summary>
/// Сервис для работы с вложениями. Картинки передаются как IFormFile, возвращаются как Base64 строка
/// </summary>
public interface IAttachmentService
{
    Task<IEnumerable<AttachmentModel>> UploadFiles(Guid userId, AttachmentModelRequest attachments);

    Task<IEnumerable<AttachmentViewModel>> GetAttachments(FileType type, Guid contentId);
    Task<IEnumerable<AvatarModel>> GetAvatars(Guid userId);
    Task DeletePostAttachment(Guid userId, Guid postId, Guid attachmentId);
    Task DeleteAvatar(Guid userId, Guid avatarId);
}