using SocialNetwork.MessengerService.Models;

namespace SocialNetwork.MessengerService;

public interface IMessengerService
{
    Task<MessageModelResponse> SendMessageToUser(Guid userId, Guid receiverId, MessageModelRequest message);

    Task<IEnumerable<MessageModelResponse>> GetMessages(Guid userId, Guid dialogId, int offset = 0, int limit = 10);
    Task<IEnumerable<ChatModelResponse>> GetChats(Guid userId, int offset = 0, int limit = 10);
}