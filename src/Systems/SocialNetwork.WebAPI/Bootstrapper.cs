using SocialNetwork.AccountServices;
using SocialNetwork.Actions;
using SocialNetwork.AttachmentServices;
using SocialNetwork.Cache;
using SocialNetwork.CommentService;
using SocialNetwork.ComplaintsServices;
using SocialNetwork.EmailService;
using SocialNetwork.GroupServices;
using SocialNetwork.MessengerService;
using SocialNetwork.PostServices;
using SocialNetwork.RabbitMq;
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
        services.AddComplaintService();
        services.AddMessengerService();
        services.AddCache();
        services.AddRabbitMq();
        services.AddActions();
        return services;
    }
}