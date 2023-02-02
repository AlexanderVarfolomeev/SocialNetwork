using System.Reflection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SocialNetwork.IdentityServer.Configuration.HealthChecks;

public class SelfHealthCheck: IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var assembly = Assembly.Load("SocialNetwork.IdentityServer");
        var versionNumber = assembly.GetName().Version;

        return Task.FromResult(HealthCheckResult.Healthy(description: $"Build {versionNumber}"));
    }
}