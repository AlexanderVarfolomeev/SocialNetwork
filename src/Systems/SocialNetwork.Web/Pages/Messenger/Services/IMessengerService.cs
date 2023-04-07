using SocialNetwork.Web.Pages.Messenger.Models;

namespace SocialNetwork.Web.Pages.Messenger.Services;

public interface IMessengerService
{
    Task<IEnumerable<ChatModel>> GetChats();
    Task<IEnumerable<MessageModel>> GetMessages(Guid chatId);
    Task<MessageModel> SendMessage(Guid receiverId, string text);
    Task<IEnumerable<UserInChatModel>> GetUsersInChat(Guid chatId);
}