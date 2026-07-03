namespace Shared;

public static class ExceptionExtensions
{
    public static string GetMessage(this Exception ex)
    {
        return ex.InnerException == null
            ? ex.Message
            : $"{ex.Message} ({ex.InnerException.Message})";
    }

    public static IEnumerable<string> GetMessages(this Exception ex)
    {
        var result = Enumerable.Repeat(ex.Message, 1);
        return AddInnerMessages(result, ex.InnerException);

        static IEnumerable<string> AddInnerMessages(IEnumerable<string> messages, Exception? innerException)
        {
            if (innerException == null)
                return messages;
            messages = messages.Append(innerException.Message);
            return AddInnerMessages(messages, innerException.InnerException);
        }
    }
}