using AutoMapper;
using Serilog;
using SocialNetwork.AccountServices.Models;
using SocialNetwork.Cache;
using SocialNetwork.Common.Enum;
using SocialNetwork.Common.Exceptions;
using SocialNetwork.Constants.Errors;
using SocialNetwork.Entities.User;
using SocialNetwork.RelationshipServices.Models;
using SocialNetwork.Repository;

namespace SocialNetwork.RelationshipServices;

public class RelationshipService : IRelationshipService
{
    private string _friendlistKey = "friend_list_key_";
    private readonly IRepository<AppUser> _userRepository;
    private readonly IMapper _mapper;
    private readonly IRepository<Relationship> _relationshipRepository;
    private readonly ICacheService _cacheService;

    public RelationshipService(
        IRepository<AppUser> userRepository,
        IMapper mapper,
        IRepository<Relationship> relationshipRepository,
        ICacheService cacheService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _relationshipRepository = relationshipRepository;
        _cacheService = cacheService;
    }

    public async Task SendFriendRequest(Guid senderId, Guid recipientId)
    {
        var fromUser = await _userRepository.GetAsync(senderId);
        var toUser = await _userRepository.GetAsync(recipientId);
        ProcessException.ThrowIf(() => fromUser.IsBanned || toUser.IsBanned, ErrorMessages.UserIsBannedError);

        var relationship = await GetRelationshipBetweenUsers(senderId, recipientId);

        if (relationship is not null)
        {
            ProcessException.ThrowIf(
                () => relationship.RelationshipType == RelationshipType.Friend ||
                      relationship.RelationshipType == RelationshipType.FriendRequest,
                ErrorMessages.RelationshipExistsError);

            // Если запрос на дружбу уже отклоняли, и запрос шлет человек, который это уже делал, то кидаем ошибку
            ProcessException.ThrowIf(
                () => relationship.RelationshipType == RelationshipType.Rejected &&
                      relationship.FirstUserId == senderId,
                ErrorMessages.UserAlreadyRejectFriendshipRequest);
        }
        else
        {
            relationship = new Relationship
            {
                FirstUserId = fromUser.Id,
                SecondUserId = toUser.Id,
                RelationshipType = RelationshipType.FriendRequest
            };

            await _relationshipRepository.AddAsync(relationship);
        }
    }

    public async Task AcceptFriendRequest(Guid recipientId, Guid requestId)
    {
        var relationship = await _relationshipRepository.GetAsync(requestId);

        ProcessException.ThrowIf(() => relationship.SecondUserId != recipientId,
            ErrorMessages.RelationshipDoesntExistsError);
        ProcessException.ThrowIf(() => relationship.RelationshipType != RelationshipType.FriendRequest,
            ErrorMessages.RequestFriendshipIrrelevantError);

        relationship.RelationshipType = RelationshipType.Friend;
        await _relationshipRepository.UpdateAsync(relationship);
        await _cacheService.Delete(_friendlistKey + recipientId);
    }

    public async Task RejectFriendRequest(Guid recipientId, Guid requestId)
    {
        var relationship = await _relationshipRepository.GetAsync(requestId);

        ProcessException.ThrowIf(() => relationship.SecondUserId != recipientId,
            ErrorMessages.RelationshipDoesntExistsError);
        ProcessException.ThrowIf(() => relationship.RelationshipType != RelationshipType.FriendRequest,
            ErrorMessages.RequestFriendshipIrrelevantError);

        relationship.RelationshipType = RelationshipType.Rejected;
        await _relationshipRepository.UpdateAsync(relationship);
    }

    public async Task<List<FriendshipRequest>> GetFriendshipRequests(Guid userId, int offset = 0, int limit = 10)
    {
        var list = await _relationshipRepository.GetAllAsync(x =>
            x.SecondUserId == userId && x.RelationshipType == RelationshipType.FriendRequest, offset, limit);
        
        return _mapper.Map<List<FriendshipRequest>>(list);
    }

    public async Task<List<AppAccountModel>> GetFriendList(Guid userId, int offset = 0, int limit = 10)
    {
        try
        {
            var cachedData = await _cacheService.Get<List<AppAccountModel>>(_friendlistKey + userId);
            if (cachedData != null)
                return cachedData;
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, ErrorMessages.GetCacheError);
        }
        
        var relationships = await _relationshipRepository.GetAllAsync(
            x => (x.FirstUserId == userId || x.SecondUserId == userId) &&
                 x.RelationshipType == RelationshipType.Friend, offset, limit);

        var friendsIds = relationships.Select(x => x.FirstUserId == userId ? x.SecondUserId : x.FirstUserId);
        var friendList = await _userRepository.GetAllAsync(x => friendsIds.Contains(x.Id));

        var data = _mapper.Map<List<AppAccountModel>>(friendList);
        await _cacheService.Put(_friendlistKey + userId, data);
        return data;
    }

    public async Task DeleteFromFriendList(Guid userId, Guid friendId)
    {
        var friendsIds = (await GetFriendList(userId)).Select(x => x.Id);
        if (friendsIds.Contains(friendId))
        {
            var relationship = await GetRelationshipBetweenUsers(friendId, userId);
            await _relationshipRepository.DeleteAsync(relationship!);
            await _cacheService.Delete(_friendlistKey + userId);
        }
        else
        {
            throw new ProcessException(ErrorMessages.RelationshipDoesntExistsError);
        }
    }

    private async Task<Relationship?> GetRelationshipBetweenUsers(Guid userId1, Guid userId2)
    {
        try
        {
            return await _relationshipRepository.GetAsync(
                x =>
                    (x.FirstUserId == userId1 && x.SecondUserId == userId2)
                    || x.FirstUserId == userId2 && x.SecondUserId == userId1);
        }
        catch (ProcessException)
        {
            return null; // УБРАТЬ
        }
    }
}