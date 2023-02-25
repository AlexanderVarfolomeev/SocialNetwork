using AutoMapper;
using SocialNetwork.Common.Exceptions;
using SocialNetwork.Constants.Errors;
using SocialNetwork.Entities.Groups;
using SocialNetwork.Entities.User;
using SocialNetwork.GroupServices.Models;
using SocialNetwork.Repository;

namespace SocialNetwork.GroupServices;

public class GroupService : IGroupService
{
    private readonly IRepository<AppUser> _userRepository;
    private readonly IRepository<Group> _groupRepository;
    private readonly IMapper _mapper;
    private readonly IRepository<UserInGroup> _userInGroupRepository;

    public GroupService(
        IRepository<AppUser> userRepository,
        IRepository<Group> groupRepository,
        IMapper mapper,
        IRepository<UserInGroup> userInGroupRepository)
    {
        _userRepository = userRepository;
        _groupRepository = groupRepository;
        _mapper = mapper;
        _userInGroupRepository = userInGroupRepository;
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
            DateOfEntry = DateTimeOffset.Now
        });

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
                DateOfEntry = DateTimeOffset.Now
            });
        }
        else
        {
            ProcessException.ThrowIf(() => subscription.IsCreator, ErrorMessages.CreatorCantUnsubscribeFromGroupError);
            await _userInGroupRepository.DeleteAsync(subscription);
        }
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