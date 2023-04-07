using Microsoft.AspNetCore.Components.Forms;
using SocialNetwork.Web.Pages.Messenger.Models;
using SocialNetwork.Web.Pages.Posts.Models;

namespace SocialNetwork.Web.Pages.Messenger.Services;

public interface IMessengerService
{
    Task<IEnumerable<ChatModel>> GetChats();
    Task<IEnumerable<MessageModel>> GetMessages(Guid chatId, int offset = 0, int limit = 10000);
    Task<MessageModel> SendMessage(Guid receiverId, string text);
    Task<IEnumerable<UserInChatModel>> GetUsersInChat(Guid chatId);
    Task<IEnumerable<AttachmentModel>> GetAttachments(Guid messageId);
    Task AddAttachments(Guid messageId, IEnumerable<IBrowserFile> files);
}