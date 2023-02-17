using System.Collections;
using Microsoft.AspNetCore.Http;
using SocialNetwork.AttachmentServices.Models;
using SocialNetwork.Common.Enum;

namespace SocialNetwork.AttachmentServices;

/// <summary>
/// Сервис для работы с вложениями. Картинки передаются как IFormFile, возвращаются как Base64 строка
/// </summary>
public interface IAttachmentService
{
    Task<IEnumerable<AttachmentModel>> UploadFiles(Guid userId, AttachmentModelRequest attachments);

    Task<IEnumerable<string>> GetAttachments(FileType type, Guid contentId);
    Task<string> GetCurrentAvatar(Guid userId);
}