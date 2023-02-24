using SocialNetwork.Entities.User;
using SocialNetwork.GroupServices.Models;

namespace SocialNetwork.GroupServices;

public interface IGroupService
{
    Task<GroupModelResponse> GetGroup(Guid groupId);
    Task<GroupModelResponse> GetGroupByName(string groupName);
    Task<IEnumerable<GroupModelResponse>> GetGroups(int offset = 0, int limit = 10);
    Task<GroupModelResponse> CreateGroup(Guid userId, GroupModelRequest groupModelRequest);
    Task<IEnumerable<UserInGroupModel>> GetSubscribers(Guid groupId, int offset = 0, int limit = 10);
    Task<IEnumerable<UserInGroupModel>> GetSubscribersByGroupName(string groupName, int offset = 0, int limit = 10);
}