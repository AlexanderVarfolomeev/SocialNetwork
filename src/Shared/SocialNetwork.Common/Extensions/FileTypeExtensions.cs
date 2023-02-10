using SocialNetwork.Common.Enum;

namespace SocialNetwork.Common.Extensions;

public static class FileTypeExtensions
{
    public static string GetPath(this FileType type) => Environment.CurrentDirectory + "\\wwwroot\\" + type;
}