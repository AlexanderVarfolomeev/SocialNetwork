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
        var usersIds =
            (await _userInGroupRepository.GetAllAsync(x => x.GroupId == groupId, offset, limit)).Select(x => x.UserId);

        return _mapper.Map<IEnumerable<UserInGroupModel>>(
            await _userRepository.GetAllAsync(x => usersIds.Contains(x.Id)));
    }

    public async Task<IEnumerable<UserInGroupModel>> GetSubscribersByGroupName(string groupName, int offset = 0,
        int limit = 10)
    {
        var group = await GetGroupByName(groupName);
        return await GetSubscribers(group.Id, offset, limit);
    }
}