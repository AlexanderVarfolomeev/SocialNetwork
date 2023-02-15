using SocialNetwork.Common.Enum;

namespace SocialNetwork.Common.Extensions;

public static class FileTypeExtensions
{
    public static string GetPath(this FileType type) => Path.Combine(new []{Environment.CurrentDirectory, "wwwroot", type.ToString()});
}