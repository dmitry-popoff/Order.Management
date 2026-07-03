namespace Shared;

public static class ResultExtensions
{
    public static T Match<T>(
        this IResult result,
        Func<T> onSuccess,
        Func<ErrorDetails, T> onFailure)
    {
        return result.IsSuccess ? onSuccess() : onFailure(result.Error);
    }
}
