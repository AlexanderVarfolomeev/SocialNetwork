namespace SocialNetwork.Constants.Errors;

public static class ErrorMessages
{
    public const string NotFoundError = "Object not found.";
    public const string YouBannedError = "You are banned.";
    public const string IncorrectPassword = "Incorrect password.";
    public const string AccessRightsError = "Not enough rights for this.";
    public const string IncorrectEmailOrPasswordError = "Incorrect email or password.";
    public const string OnlyAdminCanDoItError = "Only admin can do it.";
    public const string CantBanAdminError = "You can't ban the admin.";
    public const string OnlyAdminOrAccountOwnerCanDoIdError = "Only admin or account owner can do it.";
    public const string OnlyAccountOwnerCanDoIdError = "Only account owner can do it.";
    public const string UserWithThisEmailExistsError = "User with this email exists.";
    public const string UpdateEntityError = "Error while updating entity.";
    public const string SaveEntityError = "Error while saving entity.";
    public const string ErrorWhileResetPassword = "Error, while reset password.";
    public const string UserIsBannedError = "User is banned!";
    public const string UserIsAdminError = "User is admin.";
    public const string UserIsNotAdminError = "User is not admin.";
    public const string UserIsUnbannedError = "User is not banned.";
    public const string RelationshipExistsError = "Relationship between users already exists.";
    public const string RelationshipDoesntExistsError = "Relationship between users doesnt exists.";
    public const string RequestFriendshipIrrelevantError = "The request for friendship is irrelevant.";
    public const string UserAlreadyRejectFriendshipRequest = "The user has already rejected friendship request.";
    public const string InvestmentLimitExceededError = "The maximum number of attachments is 10!";
    public const string MorThanOneAvatarError = "You can attach only one photo for the avatar.";
    public const string UnsupportedTypeError = "Unsupported type.";
    public const string UserNotFoundError = "User not found.";
    public const string GroupNotFoundError = "Group not found.";
    public const string OnlyAdminOfGroupCanAddContentInError = "Only administrators of group can add content to the group.";
    public const string OnlyCreatorOfContentCanDoItError = "Only creator of content can do it.";
    public const string GroupWithThisNameAlreadyExistsError = "Group with this name already exists.";
    public const string CreatorCantUnsubscribeFromGroupError = "The creator of the group cannot unsubscribe from it.";
    public const string UserNotSubToGroupError = "The user is not subscribed to the group.";
    public const string SendMessageToNotFriendError = "You can only send a message to your friends";
    public const string GetCacheError = "Error while getting data from cache";
    public const string CantSendFriendRequestYourselfError = "You can't send a friend request to yourself.";
}