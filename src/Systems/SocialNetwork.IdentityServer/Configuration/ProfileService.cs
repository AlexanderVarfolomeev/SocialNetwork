using System.Security.Claims;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using SocialNetwork.Entities.User;
using SocialNetwork.Repository;

namespace SocialNetwork.IdentityServer.Configuration;

public class ProfileService : IProfileService
{
    private readonly IRepository<AppUser> _userRepository;

    public ProfileService(IRepository<AppUser> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var requestedClaimTypes = context.RequestedClaimTypes;
        var user = await _userRepository.GetAsync(Guid.Parse(context.Subject.GetSubjectId()));

        context.AddRequestedClaims(new[]
        {
            new Claim(JwtClaimTypes.NickName, user.UserName),
            new Claim(JwtClaimTypes.Email, user.Email)
        });
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        context.IsActive = true;
        return Task.CompletedTask;
    }
}