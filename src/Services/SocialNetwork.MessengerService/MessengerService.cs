using AutoMapper;
using SocialNetwork.Common.Exceptions;
using SocialNetwork.Constants.Errors;
using SocialNetwork.Entities.Messenger;
using SocialNetwork.Entities.User;
using SocialNetwork.MessengerService.Models;
using SocialNetwork.RelationshipServices;
using SocialNetwork.Repository;

namespace SocialNetwork.MessengerService;
public class MessengerService : IMessengerService
{
    private readonly IRepository<AppUser> _userRepository;
    private readonly IRepository<Message> _messageRepository;
    private readonly IRepository<Chat> _chatRepository;
    private readonly IRepository<UserInChat> _userInChatRepository;
    private readonly IRelationshipService _relationshipService;
    private readonly IMapper _mapper;

    public MessengerService(
        IRepository<AppUser> userRepository,
        IRepository<Message> messageRepository,
        IRepository<Chat> chatRepository,
        IRepository<UserInChat> userInChatRepository,
        IRelationshipService relationshipService,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _messageRepository = messageRepository;
        _chatRepository = chatRepository;
        _userInChatRepository = userInChatRepository;
        _relationshipService = relationshipService;
        _mapper = mapper;
    }

    public async Task<MessageModelResponse> SendMessageToUser(Guid userId, Guid receiverId, MessageModelRequest message)
    {
        var sender = await _userRepository.GetAsync(userId);
        var receiver = await _userRepository.GetAsync(receiverId);

        ProcessException.ThrowIf(() => sender.IsBanned, ErrorMessages.YouBannedError);
        ProcessException.ThrowIf(() => receiver.IsBanned, ErrorMessages.UserIsBannedError);

        var friends = await _relationshipService.GetFriendList(userId, 0, 1000);
        ProcessException.ThrowIf(() => !friends.Select(x => x.Id).Contains(receiverId), ErrorMessages.SendMessageToNotFriendError);

        var chat = await GetDialogBetweenUsers(userId, receiverId);
       
        var msg = new Message()
        {
            Text = message.Text,
            IsRead = false,
            IsModification = false,
            ChatId = chat.Id,
            SenderId = userId
        };

        msg = await _messageRepository.AddAsync(msg);
        return _mapper.Map<MessageModelResponse>(msg);
    }
    
    public async Task<IEnumerable<MessageModelResponse>> GetMessages(Guid userId, Guid dialogId, int offset = 0,
        int limit = 10000)
    {
        var chat = await _chatRepository.GetAsync(dialogId);
        var users = (await GetUsersInChat(dialogId)).Select(x => x.UserId);
        ProcessException.ThrowIf(() => !users.Contains(userId), ErrorMessages.AccessRightsError);

        var messages = await _messageRepository.GetAllAsync(x => x.ChatId == dialogId, offset, limit);
        return _mapper.Map<IEnumerable<MessageModelResponse>>(messages);
    }

    public async Task<IEnumerable<ChatModelResponse>> GetChats(Guid userId, int offset = 0, int limit = 10)
    {
        var chatsIds = (await _userInChatRepository.GetAllAsync(x => x.UserId == userId))
            .Select(x => x.ChatId)
            .ToList();


        var chats = (await _chatRepository.GetAllAsync(x => chatsIds.Contains(x.Id))).ToList();

        return chats.Select(x => _mapper.Map<ChatModelResponse>(x));
    }
    
    public async Task<IEnumerable<UserInChatModelResponse>> GetUsersInChat(Guid chatId)
    {
        await _chatRepository.GetAsync(chatId);
        var users = await _userInChatRepository.GetAllAsync(x => x.ChatId == chatId);
        return users.Select(x => _mapper.Map<UserInChatModelResponse>(x)).ToList();
    }

    private async Task<Chat> GetDialogBetweenUsers(Guid user1, Guid user2)
    {
        var chatsIds = (await GetChats(user1, 0, 1000)).Where(x => x.IsDialog).Select(x => x.Id).ToList();
        var users = await _userInChatRepository.GetAllAsync(x => chatsIds.Contains(x.ChatId) && x.UserId == user2);
        var chatId = chatsIds.FirstOrDefault(x => x == users.First().ChatId);
        var chat = (await _chatRepository.GetAllAsync(x => x.Id == chatId)).FirstOrDefault();
        
        if (chat is null)
        {
            chat = new Chat()
            {
                ChatName = "Dialog",
                IsDialog = true
            };

            chat = await _chatRepository.AddAsync(chat);

            var userInChat1 = new UserInChat()
            {
                ChatId = chat.Id,
                IsCreator = false,
                UserId = user1,
                EntryDate = DateTime.Now
            };
            var userInChat2 = new UserInChat()
            {
                ChatId = chat.Id,
                IsCreator = false,
                UserId = user2,
                EntryDate = DateTime.Now
            };

            await _userInChatRepository.AddAsync(userInChat1);
            await _userInChatRepository.AddAsync(userInChat2);
        }

        return chat;
    }
}