using SocialNetwork.AccountServices;
using SocialNetwork.AttachmentServices;
using SocialNetwork.CommentService;
using SocialNetwork.EmailService;
using SocialNetwork.GroupServices;
using SocialNetwork.PostServices;
using SocialNetwork.RelationshipServices;
using SocialNetwork.Repository;
using SocialNetwork.Settings;

namespace SocialNetwork.WebAPI;

public static class Bootstrapper
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddSettings();
        services.AddRepository();
        services.AddAccountService();
        services.AddEmailService();
        services.AddRelationshipService();
        services.AddAttachmentService();
        services.AddPostServices();
        services.AddGroupService();
        services.AddCommentService();
        return services;
    }
}