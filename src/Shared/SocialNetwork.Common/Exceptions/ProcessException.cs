namespace SocialNetwork.Common.Exceptions;

public class ProcessException : Exception
{
    /// <summary>
    /// Error code
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Error name
    /// </summary>
    public string Name { get; }

    public static void ThrowIf(Func<bool> predicate, string message)
    {
        if (predicate.Invoke())
            throw new ProcessException(message);
    }
    
    public static void ThrowIf(Func<bool> predicate, Exception ex)
    {
        if (predicate.Invoke())
            throw new ProcessException(ex);
    }

    #region Constructors

    public ProcessException()
    {
    }

    public ProcessException(string message) : base(message)
    {
    }

    public ProcessException(Exception inner) : base(inner.Message, inner)
    {
    }

    public ProcessException(string message, Exception inner) : base(message, inner)
    {
    }

    public ProcessException(string code, string message) : base(message)
    {
        Code = code;
    }

    public ProcessException(string code, string message, Exception inner) : base(message, inner)
    {
        Code = code;
    }

    #endregion
}