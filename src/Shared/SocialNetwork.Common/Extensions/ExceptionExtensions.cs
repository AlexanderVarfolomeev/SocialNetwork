using System.ComponentModel.DataAnnotations;
using SocialNetwork.Common.Exceptions;
using SocialNetwork.Common.Responses;

namespace SocialNetwork.Common.Extensions;

public static class ExceptionExtensions
{
    //TODO обработка валидации
    public static ErrorResponse ToErrorResponse(this ProcessException data)
    {
        var res = new ErrorResponse
        {
            Message = data.Message
        };

        return res;
    }

    public static ErrorResponse ToErrorResponse(this Exception data)
    {
        var res = new ErrorResponse
        {
            Message = data.Message
        };

        return res;
    }
}