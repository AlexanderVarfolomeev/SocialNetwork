using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SocialNetwork.AccountServices.Interfaces;
using SocialNetwork.AccountServices.Models;
using SocialNetwork.Common.Enum;
using SocialNetwork.Common.Exceptions;
using SocialNetwork.Constants.Errors;
using SocialNetwork.EmailService;
using SocialNetwork.Entities.User;
using SocialNetwork.RelationshipServices.Models;
using SocialNetwork.Repository;
using SocialNetwork.Settings.Interfaces;

namespace SocialNetwork.RelationshipServices;

public class RelationshipService : IRelationshipService
{
    private readonly IRepository<AppUser> _userRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IMapper _mapper;
    private readonly IAppSettings _apiSettings;
    private readonly IRepository<Relationship> _relationshipRepository;
    private readonly IRepository<AppUserRole> _userRolesRepository;
    private readonly IRepository<AppRole> _roleRepository;
    private readonly IEmailService _emailService;
    private readonly IAdminService _adminService;

    private readonly Guid _currentUserId;

    public RelationshipService(IRepository<AppUser> userRepository, UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager, IMapper mapper, IAppSettings apiSettings,
        IRepository<Relationship> relationshipRepository,
        IRepository<AppUserRole> userRolesRepository, IRepository<AppRole> roleRepository,
        IEmailService emailService, IHttpContextAccessor accessor, IAdminService adminService)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _apiSettings = apiSettings;
        _relationshipRepository = relationshipRepository;
        _userRolesRepository = userRolesRepository;
        _roleRepository = roleRepository;
        _emailService = emailService;
        _adminService = adminService;

        var value = accessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _currentUserId = value is null ? Guid.Empty : Guid.Parse(value);
    }

    public async Task SendFriendRequest(Guid userId)
    {
        var fromUser = await _userRepository.GetAsync(_currentUserId);
        var toUser = await _userRepository.GetAsync(userId);
        ProcessException.ThrowIf(() => fromUser.IsBanned || toUser.IsBanned, ErrorMessages.UserIsBannedError);

        var relationship = (
            await _relationshipRepository.GetAllAsync(
                (x) => x.FirstUserId == fromUser.Id && x.SecondUserId == toUser.Id)).FirstOrDefault();

        if (relationship is not null)
        {
            if (relationship.RelationshipType == RelationshipType.Friend ||
                relationship.RelationshipType == RelationshipType.FriendRequest)
            {
                throw new ProcessException(ErrorMessages.RelationshipExistsError);
            }

            // Если запрос на дружбу уже отклоняли, и запрос шлет человек, который это уже делал, то кидаем ошибку
            if (relationship.RelationshipType == RelationshipType.Rejected &&
                relationship.FirstUserId == _currentUserId)
            {
                throw new ProcessException(ErrorMessages.UserAlreadyRejectFriendshipRequest);
            }
        }


        relationship = new Relationship()
        {
            FirstUserId = fromUser.Id,
            SecondUserId = toUser.Id,
            RelationshipType = RelationshipType.FriendRequest
        };

        await _relationshipRepository.AddAsync(relationship);
    }

    public async Task AcceptFriendRequest(Guid requestId)
    {
        var relationship = await _relationshipRepository.GetAsync(requestId);

        ProcessException.ThrowIf(() => relationship.SecondUserId != _currentUserId,
            ErrorMessages.RelationshipDoesntExistsError);
        ProcessException.ThrowIf(() => relationship.RelationshipType != RelationshipType.FriendRequest,
            ErrorMessages.RequestFriendshipIrrelevantError);

        relationship.RelationshipType = RelationshipType.Friend;
        await _relationshipRepository.UpdateAsync(relationship);
    }

    public async Task RejectFriendRequest(Guid requestId)
    {
        var relationship = await _relationshipRepository.GetAsync(requestId);

        ProcessException.ThrowIf(() => relationship.SecondUserId != _currentUserId,
            ErrorMessages.RelationshipDoesntExistsError);
        ProcessException.ThrowIf(() => relationship.RelationshipType != RelationshipType.FriendRequest,
            ErrorMessages.RequestFriendshipIrrelevantError);

        relationship.RelationshipType = RelationshipType.Rejected;
        await _relationshipRepository.UpdateAsync(relationship);
    }

    public async Task<List<FriendshipRequest>> GetFriendshipRequests(int offset = 0, int limit = 10)
    {
        var list = await _relationshipRepository.GetAllAsync((x) =>
            x.SecondUserId == _currentUserId && x.RelationshipType == RelationshipType.FriendRequest, offset, limit);
        return _mapper.Map<List<FriendshipRequest>>(list);
    }

    public async Task<List<AppAccountModel>> GetFriendList(int offset = 0, int limit = 10)
    {
        var relationships = await _relationshipRepository.GetAllAsync(
            x => (x.FirstUserId == _currentUserId || x.SecondUserId == _currentUserId) &&
                 x.RelationshipType == RelationshipType.Friend, offset, limit);

        var friendsIds = relationships.Select(x => x.FirstUserId == _currentUserId ? x.SecondUserId : x.FirstUserId);
        var friendList = await _userRepository.GetAllAsync(x => friendsIds.Contains(x.Id));

        return _mapper.Map<List<AppAccountModel>>(friendList);
    }

    public async Task DeleteFromFriendList(Guid userId)
    {
        var friendsIds = (await GetFriendList()).Select(x => x.Id);
        if (friendsIds.Contains(userId))
        {
            var relationship = await _relationshipRepository.GetAsync(
                x =>
                    (x.FirstUserId == userId && x.SecondUserId == _currentUserId)
                    || x.FirstUserId == _currentUserId && x.SecondUserId == userId);

            await _relationshipRepository.DeleteAsync(relationship);
        }
        else
        {
            throw new ProcessException(ErrorMessages.RelationshipDoesntExistsError);
        }
    }
}