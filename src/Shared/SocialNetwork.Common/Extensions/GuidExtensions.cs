namespace SocialNetwork.Common.Extensions;

public static class GuidExtensions
{
    public static string Shrink(this Guid guid)
    {
        return guid.ToString().Replace("-", "").Replace(" ", "");
    }
}