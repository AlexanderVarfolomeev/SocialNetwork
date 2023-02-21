using SocialNetwork.GroupServices.Models;

namespace SocialNetwork.GroupServices;

public interface IGroupService
{
    Task<GroupModelResponse> GetGroup(Guid groupId);
    Task<IEnumerable<GroupModelResponse>> GetGroups(int offset = 0, int limit = 10);
    Task<GroupModelResponse> AddGroup(Guid userId, GroupModelRequest groupModelRequest);
}