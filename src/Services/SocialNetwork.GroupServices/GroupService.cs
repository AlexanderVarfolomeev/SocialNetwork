using AutoMapper;
using Serilog;
using SocialNetwork.Cache;
using SocialNetwork.Common.Exceptions;
using SocialNetwork.Constants.Errors;
using SocialNetwork.Entities.Groups;
using SocialNetwork.Entities.User;
using SocialNetwork.GroupServices.Models;
using SocialNetwork.Repository;

namespace SocialNetwork.GroupServices;

public class GroupService : IGroupService
{
    private string _groupListKey = "group_list_key_";
    private readonly IRepository<AppUser> _userRepository;
    private readonly IRepository<Group> _groupRepository;
    private readonly IMapper _mapper;
    private readonly IRepository<UserInGroup> _userInGroupRepository;
    private readonly ICacheService _cacheService;

    public GroupService(
        IRepository<AppUser> userRepository,
        IRepository<Group> groupRepository,
        IMapper mapper,
        IRepository<UserInGroup> userInGroupRepository,
        ICacheService cacheService)
    {
        _userRepository = userRepository;
        _groupRepository = groupRepository;
        _mapper = mapper;
        _userInGroupRepository = userInGroupRepository;
        _cacheService = cacheService;
    }

    public async Task<GroupModelResponse> GetGroup(Guid groupId)
    {
        var group = await _groupRepository.GetAsync(groupId);
        return _mapper.Map<GroupModelResponse>(group);
    }

    public async Task<GroupModelResponse> GetGroupByName(string groupName)
    {
        var group = await _groupRepository.GetAsync(x => x.Name == groupName);
        return _mapper.Map<GroupModelResponse>(group);
    }

    public async Task<IEnumerable<GroupModelResponse>> GetGroups(int offset = 0, int limit = 10)
    {
        var groups = await _groupRepository.GetAllAsync(offset, limit);
        return _mapper.Map<IEnumerable<GroupModelResponse>>(groups);
    }

    public async Task<IEnumerable<GroupModelResponse>> GetUsersGroups(Guid userId, int offset = 0, int limit = 10)
    {
        try
        {
            var cachedData = await _cacheService.Get<IEnumerable<GroupModelResponse>>(_groupListKey + userId);
            if (cachedData != null)
                return cachedData;
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, ErrorMessages.GetCacheError);
        }
        
        var groupsIds = (await _userInGroupRepository.GetAllAsync(x => x.UserId == userId)).Select(x => x.GroupId);
        var groups = await _groupRepository.GetAllAsync(x => groupsIds.Contains(x.Id),offset, limit);
        var data = _mapper.Map<IEnumerable<GroupModelResponse>>(groups).ToList();

        await _cacheService.Put(_groupListKey + userId, data);
        return data;
    }

    public async Task<GroupModelResponse> CreateGroup(Guid userId, GroupModelRequest groupModelRequest)
    {
        var user = await _userRepository.GetAsync(userId);
        ProcessException.ThrowIf(() => user.IsBanned, ErrorMessages.YouBannedError);

        var isGroupAlreadyExists = (await _groupRepository.GetAllAsync(x => x.Name == groupModelRequest.Name)).Any();
        ProcessException.ThrowIf(() => isGroupAlreadyExists, ErrorMessages.GroupWithThisNameAlreadyExistsError);

        var group = await _groupRepository.AddAsync(_mapper.Map<Group>(groupModelRequest));

        await _userInGroupRepository.AddAsync(new UserInGroup()
        {
            GroupId = group.Id,
            UserId = userId,
            IsCreator = true,
            IsAdmin = true,
            DateOfEntry = DateTime.Now
        });

        await _cacheService.Delete(_groupListKey + userId);
        return _mapper.Map<GroupModelResponse>(group);
    }

    public async Task<IEnumerable<UserInGroupModel>> GetSubscribers(Guid groupId, int offset = 0, int limit = 10)
    {
        return _mapper.Map<IEnumerable<UserInGroupModel>>(
            await _userInGroupRepository.GetAllAsync(x => x.GroupId == groupId, offset, limit));
    }

    public async Task<IEnumerable<UserInGroupModel>> GetSubscribersByGroupName(string groupName, int offset = 0,
        int limit = 10)
    {
        var group = await GetGroupByName(groupName);
        return await GetSubscribers(group.Id, offset, limit);
    }

    public async Task SubscribeToGroup(Guid userId, Guid groupId)
    {
        var user = await _userRepository.GetAsync(userId);
        ProcessException.ThrowIf(() => user.IsBanned, ErrorMessages.YouBannedError);

        var group = await _groupRepository.GetAsync(groupId);
        var subscribers = group.Users;


        // Если пользователь подписан, то отписываемся, если нет - подписываемся
        var subscription = subscribers.FirstOrDefault(x => x.UserId == userId);
        if (subscription is null)
        {
            await _userInGroupRepository.AddAsync(new UserInGroup()
            {
                GroupId = groupId,
                UserId = userId,
                IsCreator = false,
                IsAdmin = false,
                DateOfEntry = DateTime.Now
            });
        }
        else
        {
            ProcessException.ThrowIf(() => subscription.IsCreator, ErrorMessages.CreatorCantUnsubscribeFromGroupError);
            await _userInGroupRepository.DeleteAsync(subscription);
        }
        await _cacheService.Delete(_groupListKey + userId);
    }

    public async Task GrantAdminRole(Guid userId, Guid receiverId, Guid groupId)
    {
        var userInGroup = await GetUserInGroup(receiverId, groupId);
        ProcessException.ThrowIf(() => userInGroup is null, ErrorMessages.UserNotSubToGroupError);
        
        var admin = await GetUserInGroup(userId, groupId);
        ProcessException.ThrowIf(() => !admin.IsCreator, ErrorMessages.UserIsNotAdminError);
        
        userInGroup!.IsAdmin = true;
        await _userInGroupRepository.UpdateAsync(userInGroup);
    }

    public async Task RevokeAdminRole(Guid userId, Guid receiverId, Guid groupId)
    {
        var userInGroup = await GetUserInGroup(receiverId, groupId);
        ProcessException.ThrowIf(() => userInGroup is null, ErrorMessages.UserNotSubToGroupError);
        
        var admin = await GetUserInGroup(userId, groupId);
        ProcessException.ThrowIf(() => !admin.IsCreator, ErrorMessages.UserIsNotAdminError);
        
        userInGroup!.IsAdmin = false;
        await _userInGroupRepository.UpdateAsync(userInGroup);
    }

    private async Task<UserInGroup?> GetUserInGroup(Guid userId, Guid groupId)
    {
        var user = await _userRepository.GetAsync(userId);
        ProcessException.ThrowIf(() => user.IsBanned, ErrorMessages.UserIsBannedError);
        
        var group = await _groupRepository.GetAsync(groupId);
        var subscribers = group.Users;
        return subscribers.FirstOrDefault(x => x.UserId == userId);
    }
}