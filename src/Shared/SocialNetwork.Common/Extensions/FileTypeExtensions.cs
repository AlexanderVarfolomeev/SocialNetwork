using SocialNetwork.Common.Enum;

namespace SocialNetwork.Common.Extensions;

public static class FileTypeExtensions
{
    public static string GetPath(this FileType type)
    {
        return Environment.CurrentDirectory + "\\wwwroot\\" + type;
    }
}